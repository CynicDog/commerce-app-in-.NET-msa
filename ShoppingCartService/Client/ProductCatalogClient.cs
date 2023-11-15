using System.Net.Http.Headers;
using System.Text.Json;
using ShoppingCartService.Business.domain;

namespace ShoppingCartService;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCart.ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds); 
}

public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient client;
    
    // verbatim string literal, where escape characters are not interpreted, and the string is treated exactly as it appears.
    private static readonly string ProductCatalogBaseUrl = @"http://localhost:5000";
    
    // The syntax for a placeholder is {index}, where index is the zero-based index of the argument to be inserted into the format string.
    private static readonly string GetProductPathTemplate = "?productIds=[{0}]";

    public ProductCatalogClient(HttpClient client)
    {
        client.BaseAddress = new Uri(ProductCatalogBaseUrl); 
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        this.client = client; 
    }
    
    public async Task<IEnumerable<ShoppingCart.ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
    {
        using var response = await RequestProduct(productCatalogIds);
        return await ToShoppingCartItems(response); 
    }

    private async Task<HttpResponseMessage> RequestProduct(int[] productCatalogIds)
    {
        var productResource = string.Format(GetProductPathTemplate, string.Join(",", productCatalogIds));
        return await this.client.GetAsync(productResource); 
    }

    private static async Task<IEnumerable<ShoppingCart.ShoppingCartItem>> ToShoppingCartItems(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        var products = 
            await JsonSerializer.DeserializeAsync<List<ProductCatalogModel>>(
                await response.Content.ReadAsStreamAsync(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? 
            new();

        return products
            .Select(p => new ShoppingCart.ShoppingCartItem(p.ProductId, p.ProductName, p.ProductDescription, p.Price));
    }

    private record ProductCatalogModel(int ProductId, string ProductName, string ProductDescription, ShoppingCart.Money Price); 
}