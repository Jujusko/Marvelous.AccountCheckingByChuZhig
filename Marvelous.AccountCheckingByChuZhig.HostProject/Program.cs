using Marvelous.AccountCheckingByChuZhig.HostProject;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Marvelous.AccountCheckingByChuZhig.BLL;
using NLog.Extensions.Logging;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using MassTransit;
using AutoMapper;
using Marvelous.AccountCheckingByChuZhig.HostProject.Configurations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .Build();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq();
            //x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.Host("localhost", "/", h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });

            //        cfg.ConfigureEndpoints(context);
            //    });
        });

        // OPTIONAL, but can be used to configure the bus options
        services.AddOptions<MassTransitHostOptions>()
            .Configure(options =>
            {
                    // if specified, waits until the bus is started before
                    // returning from IHostedService.StartAsync
                    // default is false
                    options.WaitUntilStarted = true;

                    // if specified, limits the wait time when starting the bus
                    options.StartTimeout = TimeSpan.FromSeconds(10);

                    // if specified, limits the wait time when stopping the bus
                    options.StopTimeout = TimeSpan.FromSeconds(30);
            });

        services.AddHostedService<Worker>();
        services.AddHostedService<Sender>();
        services.AddSingleton<ILeadProducer, LeadProducer>();
        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CustomMapper());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        services.AddSingleton<ILogHelper, LogHelper>();
        services.AddSingleton<IReportService, ReportService>();
        services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
            loggingBuilder.AddNLog(config);
        });
    })
    .Build();

await host.RunAsync();
