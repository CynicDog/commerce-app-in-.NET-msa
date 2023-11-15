namespace ShoppingCartService.Business.domain;

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> items = new();
    
    // getters in Java's POJO 
    public int UserId { get; } 
    public IEnumerable<ShoppingCartItem> Items => this.items;

    public ShoppingCart(int userId) => this.UserId = userId;

    public void AddItems(IEnumerable<ShoppingCartItem> items)
    {
        foreach (var item in items)
        {
            this.items.Add(item); 
        }
    }

    public void RemoveItems(int[] productCatalogIds) =>
        this.items.RemoveWhere(i => productCatalogIds.Contains(i.ProductCatalogId));

    public record ShoppingCartItem(int ProductCatalogId, string ProductName, string Description, Money Price)
    {
        // - `virtual` specifies the method (or property, indexer) to be a base case and can be overriden in later derived classes
        // - A question mark (`?`) next to a parameter type indicates that the parameter can be nullable    
        public virtual bool Equals(ShoppingCartItem? obj) =>
            obj != null && this.ProductCatalogId.Equals(obj.ProductCatalogId);
        
        public override int GetHashCode() => this.ProductCatalogId.GetHashCode(); 
    }

    public record Money(string Currency, decimal Amount); 
}