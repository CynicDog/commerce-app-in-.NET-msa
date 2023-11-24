using System.Net.Http.Headers;
using System.Text.Json;
using ShoppingCartService.Business.domain;
using ShoppingCartService.Util;

namespace ShoppingCartService;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCart.ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds); 
}

public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient client;
    private readonly ICache cache; 
    private readonly ILogger logger;
    
    // Note 1) On `@`
    //      : verbatim string literal, where escape characters are not interpreted, and the string is treated exactly as it appears.
    // Note 2) On `host.docker.internal` 
    //      : Window/MacOS host-specific solutions to resolves to the internal IP address of the host machine. This enables containers to interact
    //      with services or applications running on the host machine as if they were running locally within the container.
    private static readonly string ProductCatalogBaseUrl = @"http://host.docker.internal:5100";
    
    // The syntax for a placeholder is {index}, where index is the zero-based index of the argument to be inserted into the format string.
    private static readonly string GetProductPathTemplate = "?productIds=[{0}]";
    
    public ProductCatalogClient(HttpClient client, ICache cache, ILogger<ProductCatalogClient> logger)
    {
        client.BaseAddress = new Uri(ProductCatalogBaseUrl); 
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        this.client = client;
        this.cache = cache; 
        this.logger = logger;
    }
    
    public async Task<IEnumerable<ShoppingCart.ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
    {
        using var response = await RequestProduct(productCatalogIds);
        return await ToShoppingCartItems(response); 
    }

    private async Task<HttpResponseMessage> RequestProduct(int[] productCatalogIds)
    {
        this.logger.LogInformation("Passed are Product IDs of {@}", productCatalogIds);
        
        var productResource = string.Format(GetProductPathTemplate, string.Join(",", productCatalogIds));
        var response = this.cache.Get(productResource) as HttpResponseMessage;

        if (response is null)
        {
            response = await this.client.GetAsync(productResource);
            cache.AddToCache(productResource, response); 
        }

        return response; 
    }

    private static async Task<IEnumerable<ShoppingCart.ShoppingCartItem>> ToShoppingCartItems(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        var products = 
            await JsonSerializer.DeserializeAsync<List<ProductCatalogModel>>(
                // Json data to be deserialized
                await response.Content.ReadAsStreamAsync(),
                // set configuration on deserialization 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? 
            new();

        return products
            .Select(p => new ShoppingCart.ShoppingCartItem(p.ProductId, p.ProductName, p.ProductDescription, p.Price));
    }

    private record ProductCatalogModel(int ProductId, string ProductName, string ProductDescription, ShoppingCart.Money Price); 
}