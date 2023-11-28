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
    private readonly ILogger<ShoppingCartController> logger; 

    // dependency injection 
    public ShoppingCartController(IShoppingCartStore shoppingCartStore, IProductCatalogClient productCatalogClient, IEventStore eventStore, ILogger<ShoppingCartController> logger)
    {
        this.shoppingCartStore = shoppingCartStore;
        this.productCatalogClient = productCatalogClient;
        this.eventStore = eventStore;
        this.logger = logger;
    }

    // http http://localhost:5000/shopping-cart/100
    [HttpGet("{userId:int}")]
    public async Task<ShoppingCart> Get(int userId)
    {
        this.logger.LogInformation("Passed is user ID of {@userId}", userId);

        return await this.shoppingCartStore.Get(userId); 
    } 

    // http POST http://localhost:5000/shopping-cart/100/items Accept:application/json Content-Type:application/json <<< '[1, 2]'
    [HttpPost("{userId:int}/items")]
    public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
    {
        this.logger.LogInformation("Passed are Product IDs of {@productIds}", productIds);
        
        var shoppingCart = await shoppingCartStore.Get(userId);
        var shoppingCartItems = await this.productCatalogClient.GetShoppingCartItems(productIds);
        
        shoppingCart.AddItems(shoppingCartItems, eventStore);
        await shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }

    // http DELETE http://localhost:5000/shopping-cart/100/items Accept:application/json Content-Type:application/json <<< '[1]'
    [HttpDelete("{userId:int}/items")]
    public async Task<ShoppingCart> Delete(int userId, [FromBody] int[] productIds)
    {
        this.logger.LogInformation("Passed are Product IDs of {@productIds}", productIds);
        
        var shoppingCart = await this.shoppingCartStore.Get(userId); 
        
        shoppingCart.RemoveItems(productIds, this.eventStore);

        await this.shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }
}