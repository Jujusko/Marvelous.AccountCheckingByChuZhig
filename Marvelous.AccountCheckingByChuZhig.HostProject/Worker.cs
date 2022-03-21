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
            Task[] tasks = new Task[5];
            int end = 50;
            int start = 1;
            while (!stoppingToken.IsCancellationRequested)
            {
                i++;
                //if (DateTime.Now.Hour == 3 && DateTime.Now.Minute == 0)
                //{
                    for (int j = 0; j < tasks.Count(); j++)
                    {
                        tasks[j] = ass.StartTasks(start, end);
                        start += 100;
                    }
                    Task.WaitAll(tasks);
                    
                //}
                await Task.Delay(200, stoppingToken);
                _logger.LogInformation("Worker running at: {time}", i);
            }
        }
    }
}