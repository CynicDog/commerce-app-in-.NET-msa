using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Business.store;

namespace ProductCatalogService.Business.controller;

public class ProductCatalogController : ControllerBase
{
    private readonly IProductStore productStore;

    // dependency injection 
    public ProductCatalogController(IProductStore productStore)
    {
        this.productStore = productStore;
    }

    [HttpGet("")]
    [ResponseCache(Duration = 864000)]
    public IEnumerable<ProductCatalogModel> Get([FromQuery] string productIds) 
        => this.productStore.GetProductByIds(
            productIds
                .Split(',')
                .Select(s => 
                    s
                        .Replace("[", "")
                        .Replace("]", ""))
                .Select(int.Parse));
}