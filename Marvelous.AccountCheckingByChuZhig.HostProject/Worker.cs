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
            while (!stoppingToken.IsCancellationRequested)
            {
                int i = 0;
                int sizePack = 25;
                _log.DoAction("LEAD VERIFICATION STARTED");

                while (true)
                {
                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    List<LeadForUpdateRole>? leadsForCheck = _reportService.GetLeadsInRange(i, sizePack).Result;

                    if (leadsForCheck is null || leadsForCheck.Count == 0)
                    {
                        _log.DoAction("LEAD VERIFICATION COMPLETED");
                        break;
                    }
                    Task task = Parallel.ForEachAsync(leadsForCheck, async (lead, token) =>
                    {
                        await Task.Run(() => StartCheckAsync(lead));
                    });
                    await task;
                    if (task.IsCompleted)
                    {
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
            CheckerRules checkerRules = new CheckerRules(_reportService);
            //try
            //{
            bool taskCheckBirthday = checkerRules.CheckLeadBirthday(lead);
            if (taskCheckBirthday)
            {
                _leadProducer.ProcessedLeads.Add(lead);
                return;
            }

            bool taskCheckCountTransactions = checkerRules.CheckCountLeadTransactionsAsync(lead);
            if (taskCheckCountTransactions)
            {
                _leadProducer.ProcessedLeads.Add(lead);
                return;
            }
            bool taskCheckDifferenceTransactions = checkerRules.CheckDifferenceWithdrawDeposit(lead);
            if (taskCheckDifferenceTransactions)
            {
                _leadProducer.ProcessedLeads.Add(lead);
                return;
            }
            //List<Task<bool>> tasks = new List<Task<bool>> { taskCheckBirthday, taskCheckCountTransactions, taskCheckDifferenceTransactions };

            //while (tasks.Count > 0)
            //{
            //    Task<bool> completed = await Task.WhenAny(tasks);
            //    if (completed.Status == TaskStatus.RanToCompletion &&
            //        completed.Result)
            //    {
            //        lead.DeservesToBeVip = true;
            //        break;
            //    }
            //    tasks.Remove(completed);
            //}
            //if (taskCheckBirthday || taskCheckCountTransactions || taskCheckDifferenceTransactions)
            //    _leadProducer.ProcessedLeads.Add(lead);
            //}

            //catch (Exception ex)
            //{
            //    lead.DeservesToBeVip = true;
            //}
            //finally
            //{
            //}
        }

        //private void CheckerRole(LeadForUpdateRole lead)
        //{
        //    if (lead.DeservesToBeVip && lead.Role == Role.Regular)
        //    {
        //        _log.DoAction($"Lead with Id = {lead.Id} got VIP status");
        //        _leadProducer.LeadsGotVip.Add(lead);
        //    }
        //    else if (!lead.DeservesToBeVip && lead.Role == Role.Vip)
        //    {
        //        _log.DoAction($"Lead with Id = {lead.Id} lost VIP status");
        //        _leadProducer.LeadsLostVip.Add(lead);
        //    }
        //}
    }
}