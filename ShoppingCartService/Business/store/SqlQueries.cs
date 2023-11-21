namespace ShoppingCartService.Business.store;

public static class SqlQueries
{
    public const string readItemsTemplateQuery =
        @"
            select ShoppingCart.ID, ProductCatalogId, ProductName, ProductDescription, Currency, Amount 
            from ShoppingCart, ShoppingCartItem
            where ShoppingCartItem.ShoppingCartId = ShoppingCart.ID and ShoppingCart.UserId = @UserId
        ";

    public const string insertShoppingCartSql =
        @"insert into ShoppingCart.dbo.ShoppingCart (UserId) output inserted.ID values (@UserId)";

    public const string deleteForShoppingCartSql =
        @"
            delete item 
            from ShoppingCart.dbo.ShoppingCartItem item 
            inner join ShoppingCart cart on item.ShoppingCartId = cart.ID and cart.UserId = @UserId";

    public const string addAllForShoppingCartSql =
        @"
            insert into ShoppingCart.dbo.ShoppingCartItem (ShoppingCartId, ProductCatalogId, ProductName, ProductDescription, Amount, Currency)
            values (@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)
        ";
}