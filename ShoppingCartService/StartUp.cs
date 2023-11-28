using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using ShoppingCartService.Business.store;
using ShoppingCartService.Business.store.impl;
using ShoppingCartService.Event;
using ShoppingCartService.Util;

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
            services.AddTransient<IEventStore, Event.impl.EventStore>();
            services.AddTransient<ICache, Cache>(); 
            
            services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(
                        //retry the request up to 3 times
                        3,   
                        // to wait before each retry is calculated (exponential backoff strategy)
                        attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)))
                    )
                .AddTransientHttpErrorPolicy(p =>
                    p.CircuitBreakerAsync(
                        // events allowance before open state 
                        5,
                        // duration of open state in minute 
                        TimeSpan.FromMinutes(1))
                    );
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

            // // database initialization performance as server starts 
            // var shoppingCartStore = app.ApplicationServices.GetService<IShoppingCartStore>();
            // shoppingCartStore?.Init();
        }
    }
}