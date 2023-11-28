using ShoppingCartService.Business.domain;
using System.Data.SqlClient;
using Dapper;

namespace ShoppingCartService.Business.store.impl;

public class ShoppingCartStore : IShoppingCartStore
{
    private readonly ILogger<ShoppingCartStore> Logger;
    private string connectionTemplate =
        @"Data Source=azuresqledge-service; Initial Catalog=ShoppingCart; User Id=sa; Password=yourStrongPassword!";
    
    public ShoppingCartStore(ILogger<ShoppingCartStore> logger)
    {
        this.Logger = logger;
    }

    public async Task<ShoppingCart> Get(int userId)
    {
        await using var connection = new SqlConnection(this.connectionTemplate);
        await connection.OpenAsync();
        
        var founds = 
            (await connection.QueryAsync(SqlQueries.readItemsTemplateQuery, new { UserId = userId })).ToList();

        // When the rows returned by a SQL query have column names equal to the property names in a class,
        // Dapper can automatically map to instances of the class.
        Logger.LogInformation("Fetching row(s) with the user identifier of [{@userId}]", userId);
        return new ShoppingCart(
            founds.FirstOrDefault()?.ID, 
            userId, 
            founds.Select(f => 
                new ShoppingCart.ShoppingCartItem(
                    (int) f.ProductCatalogId, 
                    f.ProductName, 
                    f.ProductDescription,
                    new ShoppingCart.Money(f.Currency, f.Amount))));
    }

    public async Task Save(ShoppingCart shoppingCart)
    {
        await using var connection = new SqlConnection(this.connectionTemplate);
        
        await connection.OpenAsync();
        await using (var transaction = connection.BeginTransaction())
        {
            // `output` sql clause returns the generated identity value for the inserted row
            var shoppingCartId = shoppingCart.Id ?? // null-coalescing operator 
                                 await connection.QuerySingleAsync<int>(
                                     SqlQueries.insertShoppingCartSql, 
                                     new { shoppingCart.UserId }, transaction
                                 );
            Logger.LogInformation("Inserted an instance of `ShoppingCart` with the identifier of [{@ShoppingCartId}]", shoppingCart.Id);

            // delete all previous shopping cart items
            Logger.LogInformation("Deleting an instance of `ShoppingCart` with the identifier of [{@ShoppingCartId}]", shoppingCart.Id);
            await connection.ExecuteAsync(
                SqlQueries.deleteForShoppingCartSql,
                new { UserId = shoppingCart.UserId }, transaction);

            // add current shopping cart items
            Logger.LogInformation("Inserting an instance of `ShoppingCartItem` with the content of [{@ShoppingCartItems}]", shoppingCart.Items);
            await connection.ExecuteAsync(
                SqlQueries.addAllForShoppingCartSql,
                shoppingCart.Items.Select(item => new
                {
                    ShoppingCartId = shoppingCartId,
                    ProductCatalogId= item.ProductCatalogId,
                    ProductName = item.ProductName,
                    ProductDescription = item.Description,
                    Amount = item.Price.Amount,
                    Currency = item.Price.Currency
                }), transaction
            );
            
            await transaction.CommitAsync();
        }
    }
}