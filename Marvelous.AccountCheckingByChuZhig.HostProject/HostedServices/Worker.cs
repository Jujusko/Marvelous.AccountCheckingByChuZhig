

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Worker : BackgroundService
    {
        private IWorkerHelper _workerHelper;
        public Worker(IWorkerHelper workerHelper)
        {
            _workerHelper = workerHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await _workerHelper.DoWork();
            }

        }
    }
}