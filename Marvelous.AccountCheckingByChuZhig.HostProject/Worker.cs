using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using NLog;
namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Worker : BackgroundService
    {
        private readonly ILogHelper _log;
        private readonly ILeadProducer _leadProducer;

        public Worker(ILogHelper helper, ILeadProducer leadProducer)
        {
            _log = helper;
            _leadProducer = leadProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0; 
            AccountChecking instance = new(_log);
            List<LeadModel> leadsVip = new();
            Task<List<LeadModel>>[] tasks = new Task<List<LeadModel>>[5];
            int amountOfContacts = 20;
            int firstRow = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                i++;
                await _leadProducer.SendMessage(3, Contracts.Enums.Role.Vip);
                //if (DateTime.Now.Hour == 3 && DateTime.Now.Minute == 0)
                //{
                //    for (int j = 0; j < tasks.Count(); j++)
                //    {
                //        tasks[j] = instance.StartTasks(firstRow, amountOfContacts);
                //    }
                //    Task.WaitAll(tasks);
                //    //for(int q = 0; q < tasks.Count(); q++)
                //    //{
                //    //    foreach (var ld in tasks[q].Result)
                //    //        leadsVip.Add(ld);
                //    //}
                //    //arr result

                //}
                await Task.Delay(500000, stoppingToken);
                
            }
        }
    }
}