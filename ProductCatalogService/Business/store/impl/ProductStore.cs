namespace ProductCatalogService.Business.store.impl;

public class ProductStore : IProductStore
{
    public IEnumerable<ProductCatalogModel> GetProductByIds(IEnumerable<int> productIds) 
        => productIds.Select(id => new ProductCatalogModel(id, "foo" + id, "bar", new Money(10, "USD")));
}