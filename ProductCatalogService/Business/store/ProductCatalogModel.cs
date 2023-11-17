namespace ProductCatalogService.Business.store;

public record ProductCatalogModel(int ProductId, string ProductName, string ProductDescription, Money Price);
public record Money(); 