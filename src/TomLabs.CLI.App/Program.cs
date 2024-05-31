using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TomLabs.CLI.App.Arguments;
using TomLabs.CLI.App.Services;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        var minimumLogLevel = Serilog.Events.LogEventLevel.Information;
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                minimumLogLevel = o.VerbosityLevel;
            });
        
        var logger = new LoggerConfiguration()
            .MinimumLevel.Is(minimumLogLevel)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        logging.ClearProviders();
        logging.AddSerilog(logger);
    })
    .ConfigureServices((hostContext, services) =>
    {
        // services.AddSomeService(settings => { });

        services.AddHostedService<ConsoleHostedService>();
    })
    .RunConsoleAsync();