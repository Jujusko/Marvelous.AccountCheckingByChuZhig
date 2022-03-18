using Marvelous.AccountCheckingByChuZhig.BLL.Services;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            AccountChecking ass = new();
            while (!stoppingToken.IsCancellationRequested)
            {
                i++;
                if (i == 1)
                    ass.MainLoop();
                _logger.LogInformation("Worker running at: {time}", i);
                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}