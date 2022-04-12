using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Marvelous.Contracts.ResponseModels;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class WorkerHelper : IWorkerHelper
    {
        private readonly ILogHelper _log;
        private readonly ILeadProducer _leadProducer;
        private readonly IReportService _reportService;
        private readonly IConfigAlyona _Alyona;
        public WorkerHelper(ILogHelper helper, ILeadProducer leadProducer, IReportService reportService, IConfigAlyona Alyona)
        {
            _log = helper;
            _leadProducer = leadProducer;
            _reportService = reportService;
            _Alyona = Alyona;
        }

        private async Task RunHeapLeads(List<LeadForUpdateRole> leads)
        {
            Task task = Parallel.ForEachAsync(leads, async (lead, token) =>
            {
                await StartCheckAsync(lead);
            });
            await task;
        }
        public async Task DoWork()
        {
            var token = await _Alyona.SendRequest<string>(AuthEndpoints.ApiAuth + AuthEndpoints.TokenForMicroservice, Microservice.MarvelousAuth);
            var a =  await _Alyona.SendRequest<IEnumerable<ConfigResponseModel>>(ConfigsEndpoints.Configs, Microservice.MarvelousConfigs, token.Data);
            Console.WriteLine(a.Content);
            int startRange = 0;
            int sizePack = 25;
            _log.DoAction("LEAD VERIFICATION STARTED");
            Task[] tasks = new Task[5];
            bool breaker = true;
            List<LeadForUpdateRole>? leadsForCheck;

            while (breaker)
            {
                for (int j = 0; j < tasks.Count(); j++)
                {
                    leadsForCheck = await _reportService.GetLeadsInRange(startRange, sizePack);
                    if (leadsForCheck is null || leadsForCheck.Count == 0)
                    {
                        _log.DoAction("LEAD VERIFICATION COMPLETED");
                        breaker = false;
                        break;
                    }
                    tasks[j] = RunHeapLeads(leadsForCheck);
                    startRange += sizePack;
                }
                Task.WaitAll(tasks);
            }
        }

        private async Task StartCheckAsync(LeadForUpdateRole lead)
        {
            CheckerRules checkerRules = new CheckerRules(_reportService);

            Task<bool> taskCheckBirthday = Task.Run(() => checkerRules.CheckLeadBirthday(lead));
            Task<bool> taskCheckCountTransactions = Task.Run(() => checkerRules.CheckCountLeadTransactionsAsync(lead));
            Task<bool> taskCheckDifferenceTransactions = Task.Run(() => checkerRules.CheckDifferenceWithdrawDeposit(lead));

            List<Task<bool>> tasks = new List<Task<bool>> { taskCheckBirthday, taskCheckCountTransactions, taskCheckDifferenceTransactions };

            while (tasks.Count > 0)
            {
                Task<bool> completed = await Task.WhenAny(tasks);
                if (completed.Status == TaskStatus.RanToCompletion &&
                    completed.Result)
                {
                    lead.DeservesToBeVip = true;
                    break;
                }
                tasks.Remove(completed);
            }
            _leadProducer.ProcessedLeads.Add(lead);
        }

    }
}
