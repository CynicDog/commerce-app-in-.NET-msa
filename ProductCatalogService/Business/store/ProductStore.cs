namespace ProductCatalogService.Business.store;

public interface IProductStore
{
    IEnumerable<ProductCatalogModel> GetProductByIds(IEnumerable<int> productIds); 
}