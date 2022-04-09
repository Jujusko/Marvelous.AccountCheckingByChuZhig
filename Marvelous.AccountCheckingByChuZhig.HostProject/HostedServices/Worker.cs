using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;

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