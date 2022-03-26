using Marvelous.AccountCheckingByChuZhig.HostProject;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Marvelous.AccountCheckingByChuZhig.BLL;
using NLog.Extensions.Logging;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .Build();
        services.AddHostedService<Worker>();
        services.AddSingleton<ILeadProducer, LeadProducer>();
        services.AddSingleton<ILogHelper, LogHelper>()
        .AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddNLog(config);
        });
    })
    .Build();

await host.RunAsync();
