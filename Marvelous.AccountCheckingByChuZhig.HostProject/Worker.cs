using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using NLog;
using Marvelous.Contracts;
namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Worker : BackgroundService
    {
        private readonly ILogHelper _log;
        private readonly ILeadProducer _leadProducer;
        private readonly IReportService _reportService;

        public Worker(ILogHelper helper, ILeadProducer leadProducer, IReportService reportService)
        {
            _log = helper;
            _leadProducer = leadProducer;
            _reportService = reportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0; 
            AccountChecking instance = new(_log);
            List<LeadModel> leadsVip = new();
            Task<List<LeadModel>>[] tasks = new Task<List<LeadModel>>[5];

            int amountOfContacts = 20;
            int firstRow = 1;
            int maxCount = 10000;//from report stprocedure
            string role;
            bool endLead = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                i++;
                Console.WriteLine(DateTime.Now);
                if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 29)
                    endLead = true;
                if (endLead)
                {
                    _log.DoAction("Service started to check all leads");
                    while (firstRow + amountOfContacts < maxCount)
                    {
                        for (int j = 0; j < tasks.Count(); j++)
                        {
                            tasks[j] = instance.StartTasks(firstRow, amountOfContacts);
                            firstRow += amountOfContacts;
                        }

                        Task.WaitAll(tasks);
                        for (int q = 0; q < tasks.Count(); q++)
                        {
                            foreach (var ld in tasks[q].Result)
                            {
                                leadsVip.Add(ld);
                                if (ld.Role == Contracts.Enums.Role.Regular.ToString())
                                    role = Contracts.Enums.Role.Vip.ToString();
                                else
                                    role = Contracts.Enums.Role.Regular.ToString();
                                _log.DoAction($"Leads role with ID {ld.Id} changed from {ld.Role} to {role}");
                            }
                        }
                        //arr result
                    }
                    _log.DoAction("Service end to check all leads");
                    endLead = false;
                }
                await Task.Delay(1000, stoppingToken);
                
            }
        }

        public void StartCheck(LeadModel lead)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CheckerRules checkerRules = new(cancelTokenSource);

            Parallel.Invoke( new ParallelOptions { CancellationToken = cancelTokenSource.Token },
                            () => checkerRules.CheckLeadBirthday(lead),
                            () => checkerRules.CheckCountLeadTransactions(42),
                            () => checkerRules.CheckDifferenceWithdrawDeposit(new List<TransactionResponseModel>()));
        }
    }
}