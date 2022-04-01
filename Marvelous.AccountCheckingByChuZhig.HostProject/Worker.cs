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
            #region DD
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
            #endregion
            /*
             получать в потоках первого уровня пачками лидов
             для каждого потока создавать лист лидов, у которых что-то может поменяться
             прогонять их через StartCheckAsync в потоках второго уровня
             в потоках третьего уровня смотреть правила, и если там где-то выпал TRUE, то ставить Vip, если не стоит
             после потоков третьего уровня...
             */
            List<LeadModel> leadsVip = new();
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            List<LeadModel>? leadsForCheck = await _reportService.NewGetAllLeads(0, 5, cancelTokenSource);
            ParallelLoopResult result = Parallel.ForEach(leadsForCheck, lead => StartCheckAsync(lead));
            #region DD2
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
            #endregion
        }

        public async Task StartCheckAsync(LeadModel lead)
        {
            //int? numTask = Task.CurrentId;
            //Console.WriteLine("Поток номер - " + numTask + " запущен. Лид " + lead.Id);
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

            CheckerRules checkerRules = new(cancelTokenSource, lead);
            try
            {
                Task t3 = Task.Run(async () => checkerRules.CheckDifferenceWithdrawDeposit(await _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(lead.Id, cancelTokenSource)), cancelTokenSource.Token);
                Task t1 = Task.Run(() => checkerRules.CheckLeadBirthday(lead), cancelTokenSource.Token);
                Task t2 = Task.Run(async () => checkerRules.CheckCountLeadTransactions(await _reportService.GetCountLeadTransactionsWithoutWithdrawal(lead.Id, DateTime.Now.AddMonths(-2), cancelTokenSource)), cancelTokenSource.Token);
                

                Task[] tasks = { t1, t2, t3 };

                //Parallel.Invoke(new ParallelOptions { CancellationToken = cancelTokenSource.Token },
                //                () => checkerRules.CheckLeadBirthday(lead),
                //                async () => checkerRules.CheckCountLeadTransactions(await _reportService.GetCountLeadTransactionsWithoutWithdrawal(lead.Id, DateTime.Now.AddMonths(-2), cancelTokenSource)),
                //                async () => checkerRules.CheckDifferenceWithdrawDeposit(await _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(lead.Id, cancelTokenSource)));
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("Поток номер - " + numTask + " отработал корректно");
                //Console.ResetColor();
                await Task.WhenAll(tasks);
                Console.WriteLine("Результат проверки лида с ID = " + lead.Id + ": " + checkerRules.DeservesToBeVip);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Перехвачена " + ex.Message + " остановка потока " + " на лиде " + lead.Id);
                Console.ResetColor();
                cancelTokenSource.Dispose();//освобождение потоков
            }
            finally
            {
                //cancelTokenSource.Token.Register(() => Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa"));
            }
        }
    }
}