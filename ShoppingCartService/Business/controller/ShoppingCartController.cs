using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Business.domain;
using ShoppingCartService.Business.store;

namespace ShoppingCartService.Business.controller;

[Route("/shopping-cart")]
public class ShoppingCartController : ControllerBase
{
    // dependency injection 
    private readonly IShoppingCartStore shoppingCartStore;

    public ShoppingCartController(IShoppingCartStore shoppingCartStore)
    {
        this.shoppingCartStore = shoppingCartStore; 
    }

    [HttpGet("{userId:int}")]
    public ShoppingCart Get(int userId) => this.shoppingCartStore.Get(userId); 
}