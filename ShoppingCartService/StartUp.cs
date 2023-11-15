using ShoppingCartService.Business.store;
using ShoppingCartService.Business.store.impl;

namespace ShoppingCartService 
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // dependency injection lifetime options: 
            //      1. Transient: A new instance is created every time the service is requested.
            //      2. Scoped: A single instance is created for each scope. A scope is usually equivalent to the lifetime of a single HTTP request.
            //      3. Singleton: A single instance is created for the entire lifetime of the application. 
            services.AddTransient<IShoppingCartStore, ShoppingCartStore>(); 
        }
 
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
            
            // a delegate is a type that represents a reference to a method 
            // e.g.) `
            //          Action greet = () => Console.WriteLine("Hello, World!");
            //          greet();
            //       `

            // database initialization performance as server starts 
            var shoppingCartStore = app.ApplicationServices.GetService<IShoppingCartStore>();
            shoppingCartStore?.Init();
        }
    }
}