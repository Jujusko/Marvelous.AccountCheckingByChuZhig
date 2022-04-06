using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using NLog;
using Marvelous.Contracts;
using Marvelous.Contracts.ExchangeModels;
using Marvelous.Contracts.Enums;
using Marvelous.Producers;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Worker : BackgroundService
    {
        private readonly ILogHelper _log;
        private readonly ILeadProducer _leadProducer;
        private readonly IReportService _reportService;
        private readonly ICheckerRules _checkerRules;
        private List<LeadForUpdateRole> _leadForUpdateRoles;

        public Worker(ILogHelper helper, ILeadProducer leadProducer, IReportService reportService, ICheckerRules checkerRules)
        {
            _log = helper;
            _leadProducer = leadProducer;
            _reportService = reportService;
            _checkerRules = checkerRules;
            _leadForUpdateRoles = new();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //в будущем одна строка, а щас что имеем
            var cts = new CancellationTokenSource();
            var countOfLeadsVip = await _reportService.GetCountOfLeadsByRole(Role.Vip, cts);
            var countOfLeadsRegular = await _reportService.GetCountOfLeadsByRole(Role.Regular, cts);
            var countOfLeads = countOfLeadsVip + countOfLeadsRegular;

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
            //while (!stoppingToken.IsCancellationRequested) //прилажка крутится
            //{
            int i = 0;
            int sizePack = 10;
            for (; i <= countOfLeads; i += sizePack)
            {
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                List<LeadForUpdateRole>? leadsForCheck = await _reportService.NewGetAllLeads(i, i + sizePack, cancelTokenSource);

                await Parallel.ForEachAsync(leadsForCheck, async (lead, token) =>
                    {
                        StartCheck(lead);
                    });
            }
            //}

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

        public void StartCheck(LeadForUpdateRole lead)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            try
            {
                //Task t3 = Task.Run(async () => _checkerRules
                //.CheckDifferenceWithdrawDeposit(lead, await _reportService
                //.GetLeadTransactionsDepositWithdrawForLastMonth(lead.Id, cancelTokenSource), cancelTokenSource), cancelTokenSource.Token);
                //Task t1 = Task.Run(() => _checkerRules.CheckLeadBirthday(lead, cancelTokenSource), cancelTokenSource.Token);
                //Task t2 = Task.Run(async () => _checkerRules.CheckCountLeadTransactions(lead, await _reportService.GetCountLeadTransactionsWithoutWithdrawal(lead.Id, DateTime.Now.AddMonths(-2), cancelTokenSource), cancelTokenSource), cancelTokenSource.Token);
                //Task[] tasks = { t1, t2, t3 };

                //await Task.WhenAll(tasks);
                Parallel.Invoke(new ParallelOptions { CancellationToken = cancelTokenSource.Token },
                    ()=> _checkerRules.CheckLeadBirthday(lead, cancelTokenSource),
                    async ()=> _checkerRules.CheckCountLeadTransactions(lead, await _reportService.GetCountLeadTransactionsWithoutWithdrawal(lead.Id, DateTime.Now.AddMonths(-2), cancelTokenSource), cancelTokenSource),
                    async ()=> _checkerRules
                    .CheckDifferenceWithdrawDeposit(lead, await _reportService
                    .GetLeadTransactionsDepositWithdrawForLastMonth(lead.Id, cancelTokenSource), cancelTokenSource)
                    );
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Перехвачена " + ex.Message + " остановка потока " + " на лиде " + lead.Id);
                Console.ResetColor();
            }
            finally
            {
                //cancelTokenSource.Token.Register(() => cancelTokenSource.Dispose());
            }
        }

        //хочу чтобы этот метод крутился асинхронно и ловил лидов и в случае чего их отправлял
        private async Task CreateListAndSend(LeadForUpdateRole lead)
        {
            while (true)
            {
                if (lead is not null)
                {
                    _leadForUpdateRoles.Add(lead);
                }
            }
        }
    }
}