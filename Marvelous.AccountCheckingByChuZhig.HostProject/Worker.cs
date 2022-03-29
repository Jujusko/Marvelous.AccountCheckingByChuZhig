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
            //int i = 0; 
            //AccountChecking instance = new(_log);
            //List<LeadModel> leadsVip = new();
            //Task<List<LeadModel>>[] tasks = new Task<List<LeadModel>>[5];

            //int amountOfContacts = 20;
            //int firstRow = 1;
            //int maxCount = 10000;//from report stprocedure
            //string role;
            //bool endLead = false;
            //while (!stoppingToken.IsCancellationRequested)
            //{
            List<LeadModel> leads = new List<LeadModel>
            {
                new LeadModel { Id=5, BirthDate = new DateTime(2002, 3, 15)},
                new LeadModel { Id=2, BirthDate = new DateTime(2002, 3, 3)},
                new LeadModel { Id=3, BirthDate = new DateTime(2002, 9, 15)},
                new LeadModel { Id=7, BirthDate = new DateTime(1980, 3, 25)}
            };
            Parallel.ForEach(leads, /*async*/ lead => /*await */StartCheckAsync(lead));
            //i++;
            //Console.WriteLine(DateTime.Now);
            //if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 29)
            //    endLead = true;
            //if (endLead)
            //{
            //    _log.DoAction("Service started to check all leads");
            //    while (firstRow + amountOfContacts < maxCount)
            //    {
            //        for (int j = 0; j < tasks.Count(); j++)
            //        {
            //            tasks[j] = instance.StartTasks(firstRow, amountOfContacts);
            //            firstRow += amountOfContacts;
            //        }

            //        Task.WaitAll(tasks);
            //        for (int q = 0; q < tasks.Count(); q++)
            //        {
            //            foreach (var ld in tasks[q].Result)
            //            {
            //                leadsVip.Add(ld);
            //                if (ld.Role == Contracts.Enums.Role.Regular.ToString())
            //                    role = Contracts.Enums.Role.Vip.ToString();
            //                else
            //                    role = Contracts.Enums.Role.Regular.ToString();
            //                _log.DoAction($"Leads role with ID {ld.Id} changed from {ld.Role} to {role}");
            //            }
            //        }
            //        //arr result
            //    }
            //    _log.DoAction("Service end to check all leads");
            //    endLead = false;
            //}
            // await Task.Delay(1000, stoppingToken);

            //}
        }

        public void StartCheckAsync(LeadModel lead)
        {
            int? numTask = Task.CurrentId;
            Console.WriteLine("Поток номер - " + numTask + " запущен. Лид " + lead.Id);
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CheckerRules checkerRules = new(cancelTokenSource, lead);
            //var countTransactions = await _reportService.GetCountLeadTransactionsWithoutWithdraw(lead.Id);
            //var trans = await _reportService.GetLeadTransactionsForPeriod(lead.Id, DateTime.Now.AddDays(-10), DateTime.Now);

            //запуск проверок в разных потоках
            try
            {
                Parallel.Invoke(new ParallelOptions { CancellationToken = cancelTokenSource.Token },
                                () => checkerRules.CheckLeadBirthday(lead),
                                async () => checkerRules.CheckCountLeadTransactions(await _reportService.GetCountLeadTransactionsWithoutWithdrawal(lead.Id, DateTime.Now.AddMonths(-2))),
                                async () => checkerRules.CheckDifferenceWithdrawDeposit(await _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(lead.Id)));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Поток номер - " + numTask + " отработал корректно");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Перехвачена "+ex.Message+" остановка потока " + numTask + " на лиде " + lead.Id /*+ " " + lead.BirthDate.ToString("D")*/);
                Console.ResetColor();
                cancelTokenSource.Dispose();//освобождение потоков
            }
        }
    }
}