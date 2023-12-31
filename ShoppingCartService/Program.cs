using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using ShoppingCartService;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, logger) =>
        {
            logger
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .WriteTo.ColoredConsole(
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {TraceId} {Level:u3} {Message}{NewLine}{Exception}"
                );
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });