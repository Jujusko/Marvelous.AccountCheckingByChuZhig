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
        private ListLeadsForUpdateRole _leadsForUpdateRole = new ListLeadsForUpdateRole() { Leads = new() };

        public Worker(ILogHelper helper, ILeadProducer leadProducer, IReportService reportService)
        {
            _log = helper;
            _leadProducer = leadProducer;
            _reportService = reportService;
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
            int sizePack = 50;
            for (; i <= countOfLeads; i += sizePack)
            {
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                List<LeadModel>? leadsForCheck = await _reportService.NewGetAllLeads(i, sizePack, cancelTokenSource);

                await Parallel.ForEachAsync(leadsForCheck, async (lead, token) =>
                    {
                        await StartCheckAsync(lead);
                    });
                sizePack += sizePack;
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

        public async Task StartCheckAsync(LeadModel lead)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

            CheckerRules checkerRules = new(cancelTokenSource, lead);
            try
            {
                Task t3 = Task.Run(async () => checkerRules.CheckDifferenceWithdrawDeposit(await _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(lead.Id, cancelTokenSource)), cancelTokenSource.Token);
                Task t1 = Task.Run(() => checkerRules.CheckLeadBirthday(lead), cancelTokenSource.Token);
                Task t2 = Task.Run(async () => checkerRules.CheckCountLeadTransactions(await _reportService.GetCountLeadTransactionsWithoutWithdrawal(lead.Id, DateTime.Now.AddMonths(-2), cancelTokenSource)), cancelTokenSource.Token);
                Task[] tasks = { t1, t2, t3 };

                await Task.WhenAll(tasks);
                await CreateListAndSend(lead, checkerRules.DeservesToBeVip);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Перехвачена " + ex.Message + " остановка потока " + " на лиде " + lead.Id);
                Console.ResetColor();
            }
            finally
            {
                cancelTokenSource.Token.Register(() => cancelTokenSource.Dispose());
            }
        }

        //хочу чтобы этот метод крутился асинхронно и ловил лидов и в случае чего их отправлял
        private async Task CreateListAndSend(LeadModel lead, bool isVip)
        {
            if (isVip && lead.Role != Role.Vip.ToString())
            {
                _leadsForUpdateRole.Leads.Add(new LeadShortExchangeModel { Email = lead.Email, Id = lead.Id, Role = Role.Vip });
                _log.DoAction($"The lead with ID = {lead.Id} has been assigned VIP status");
            }
            else if (!isVip && lead.Role == Role.Vip.ToString())
            {
                _leadsForUpdateRole.Leads.Add(new LeadShortExchangeModel { Email = lead.Email, Id = lead.Id, Role = Role.Regular });
                _log.DoAction($"VIP status has been removed from the user with ID = {lead.Id}");
            }
            if (_leadsForUpdateRole.Leads.Count == 30)
            {
                await _leadProducer.SendLeads(_leadsForUpdateRole);
                _leadsForUpdateRole.Leads.Clear();
            }
        }
    }
}