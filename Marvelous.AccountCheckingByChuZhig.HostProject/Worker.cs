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
        private List<LeadForUpdateRole> _leadForUpdateRoles;

        public Worker(ILogHelper helper, ILeadProducer leadProducer, IReportService reportService)
        {
            _log = helper;
            _leadProducer = leadProducer;
            _reportService = reportService;
            _leadForUpdateRoles = new();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //в будущем одна строка, а щас что имеем
            //var cts = new CancellationTokenSource();
            //var countOfLeadsVip = await _reportService.GetCountOfLeadsByRole(Role.Vip, cts);
            //var countOfLeadsRegular = await _reportService.GetCountOfLeadsByRole(Role.Regular, cts);
            //var countOfLeads = countOfLeadsVip + countOfLeadsRegular;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                int i = 0;
                int sizePack = 5;
                _log.DoAction("LEAD VERIFICATION STARTED");

                while (true)
                {
                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    List<LeadForUpdateRole>? leadsForCheck = await _reportService.NewGetAllLeads(i, sizePack, cancelTokenSource);

                    if (leadsForCheck is null || leadsForCheck.Count == 0)
                    {
                        _log.DoAction("LEAD VERIFICATION COMPLETED");
                        break;
                    }
                    Task task = Parallel.ForEachAsync(leadsForCheck, async (lead, token) =>
                    {
                        await StartCheckAsync(lead);
                    });
                    await task;
                    if (task.IsCompleted)
                    {
                        //foreach (var lead in leadsForCheck)
                        //{
                        //    CheckerRole(lead);
                        //}
                        i += sizePack;
                    }
                }
            }

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

        public async Task StartCheckAsync(LeadForUpdateRole lead)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CheckerRules checkerRules = new CheckerRules(_reportService, cancelTokenSource);
            try
            {
                //Parallel.Invoke(
                //    new ParallelOptions { CancellationToken = cancelTokenSource.Token },
                //    async () => await checkerRules.CheckCountLeadTransactionsAsync(lead),
                //    async () => await checkerRules.CheckDifferenceWithdrawDeposit(lead),
                //    () => checkerRules.CheckLeadBirthday(lead)
                //    );

                Task taskCheckBirthday = Task.Run(() => checkerRules.CheckLeadBirthday(lead), cancelTokenSource.Token);
                Task taskCheckCountTransactions = Task.Run(() => checkerRules.CheckCountLeadTransactionsAsync(lead), cancelTokenSource.Token);
                Task taskCheckDifferenceTransactions = Task.Run(() => checkerRules.CheckDifferenceWithdrawDeposit(lead), cancelTokenSource.Token);
                Task[] tasks = { taskCheckBirthday, taskCheckCountTransactions, taskCheckDifferenceTransactions };
                await Task.WhenAll(tasks);
                await CheckerRole(lead);
            }
           
            catch (Exception ex)
            {
                lead.DeservesToBeVip = true;
            }
            finally
            {
                
             }
        }

        //private void CheckerRole(LeadForUpdateRole lead)
        //{
        //    if (lead.DeservesToBeVip && lead.Role == Role.Regular)
        //        _log.DoAction($"Lead with Id = {lead.Id} got VIP status");
        //    else if (!lead.DeservesToBeVip && lead.Role == Role.Vip) 
        //        _log.DoAction($"Lead with Id = {lead.Id} lost VIP status");
        //}
        private async Task CheckerRole(LeadForUpdateRole lead)
        {
                if (lead.DeservesToBeVip && lead.Role == Role.Regular)
                {
                    _log.DoAction($"Lead with Id = {lead.Id} got VIP status");
                    await _leadProducer.SendLeads(lead);
                }
                else if (!lead.DeservesToBeVip && lead.Role == Role.Vip)
                {
                    _log.DoAction($"Lead with Id = {lead.Id} lost VIP status");
                    await _leadProducer.SendLeads(lead);
                }
        }

        //private async Task SendLeads()
        //{
        //    while (true)
        //    {
        //        if (_leadForUpdateRoles.Count == 200)
        //        {
        //            foreach (var lead in _leadForUpdateRoles)
        //            {
        //                if (!CheckerRole(lead))
        //                {
        //                    _leadForUpdateRoles.Remove(lead);
        //                }
        //            }
        //            await _leadProducer.SendLeads(_leadForUpdateRoles);
        //            _leadForUpdateRoles.Clear();
        //        }
        //    }
        //}
    }
}