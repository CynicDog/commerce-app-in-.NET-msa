namespace ShoppingCartService 
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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
        }
    }
}