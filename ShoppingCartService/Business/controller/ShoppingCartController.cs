using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Business.domain;
using ShoppingCartService.Business.store;
using ShoppingCartService.Event;

namespace ShoppingCartService.Business.controller;

[Route("/shopping-cart")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartStore shoppingCartStore;
    private readonly IProductCatalogClient productCatalogClient;
    private readonly IEventStore eventStore;
    private readonly ILogger logger; 

    // dependency injection 
    public ShoppingCartController(IShoppingCartStore shoppingCartStore, IProductCatalogClient productCatalogClient, IEventStore eventStore, ILogger<ShoppingCartController> logger)
    {
        this.shoppingCartStore = shoppingCartStore;
        this.productCatalogClient = productCatalogClient;
        this.eventStore = eventStore;
        this.logger = logger;
    }

    [HttpGet("{userId:int}")]
    public ShoppingCart Get(int userId)
    {
        this.logger.LogInformation("Passed is user ID of {@}", userId);
        
        return this.shoppingCartStore.Get(userId); 
    } 

    [HttpPost("{userId:int}/items")]
    public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
    {
        this.logger.LogInformation("Passed are Product IDs of {@}", productIds);
        
        var shoppingCart = shoppingCartStore.Get(userId);
        var shoppingCartItems = await this.productCatalogClient.GetShoppingCartItems(productIds);
        
        shoppingCart.AddItems(shoppingCartItems, eventStore);
        shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }
}