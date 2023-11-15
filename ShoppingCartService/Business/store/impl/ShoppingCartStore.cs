using ShoppingCartService.Business.domain;

namespace ShoppingCartService.Business.store.impl;

public class ShoppingCartStore : IShoppingCartStore
{
    // in-memory database implementation for now 
    private static readonly Dictionary<int, ShoppingCart> Database = new Dictionary<int, ShoppingCart>();

    public ShoppingCart Get(int userId) => 
        Database.ContainsKey(userId) ? Database[userId] : new ShoppingCart(userId); 
    
    public void Save(ShoppingCart shoppingCart)
    {
        Database[shoppingCart.UserId] = shoppingCart; 
    }

    // public void Init()
    // {
    //     ShoppingCart shoppingCart = new ShoppingCart(0001);
    //     
    //     var item = new ShoppingCart.ShoppingCartItem(
    //         ProductCatalogId: 1000, 
    //         ProductName: "Sample Item", 
    //         Description: "Description for Sample Item",
    //         Price: new ShoppingCart.Money("USD", 100));
    //     
    //     shoppingCart.AddItems(new List<ShoppingCart.ShoppingCartItem> { item });
    //
    //     Save(shoppingCart); 
    // } 
}