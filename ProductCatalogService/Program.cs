using ProductCatalogService;
using Serilog;
using Serilog.Enrichers.Span;

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
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });