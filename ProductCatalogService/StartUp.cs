using ProductCatalogService.Business.store;
using ProductCatalogService.Business.store.impl;

namespace ProductCatalogService;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddTransient<IProductStore, ProductStore>();
    } 
 
    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
            endpoints.MapControllers());
    }
}