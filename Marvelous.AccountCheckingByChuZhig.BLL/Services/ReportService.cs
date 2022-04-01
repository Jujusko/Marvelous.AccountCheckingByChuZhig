using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly ILogHelper _logger;
        public ReportService(ILogHelper logger)
        {
            _domain = ReportUrls.ReportDomain;
            _logger = logger;
        }

        #region AUF выкатываем со дворов
        public async Task<List<TransactionResponseModel>?> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate)
        {
            Console.WriteLine("СКАЧИВАНИЕ транзакций лида с айди " + leadId);
            var request = new RestRequest("api/Transactions/by-lead-id/in-range/", Method.Get)
                .AddParameter("leadId", leadId)
                .AddParameter("startDate", startDate.ToString("s"))
                .AddParameter("endDate", endDate.ToString("s"));
            var result = await GetResponseAsync<List<TransactionResponseModel>>(request, new CancellationTokenSource());
            Console.WriteLine("Транзакции СКАЧАНЫ у лида с айди " + leadId + " их " + result.Data.Count());
            return result.Data;
        }


        public async Task<List<LeadModel>?> NewGetAllLeads(int start, int amount, CancellationTokenSource cancellationTokenSource)
        {
            RestRequest request = new RestRequest("api/Leads/take-leads-in-range", Method.Get);

            request.AddParameter("offset", start);
            request.AddParameter("fetch", amount);
            var sres = await GetResponseAsync<List<LeadModel>>(request, cancellationTokenSource);

            var list = sres.Data;


            return list;

        }

        public async Task<int> GetCountRANDOmLeadTransactionsWithoutWithdraw(int leadId)
        {
            Console.WriteLine("Получение РАНДОМНОГО кол-ва транзакций лида " + leadId);
            Random random = new Random();
            var countTransactions = await Task<int>.Run(() => random.Next(32, 52));
            Console.WriteLine("РАНДОМНЫХ транзакций лида " + leadId + " составляет " + countTransactions);
            return countTransactions;
        }
        #endregion

        public async Task<int> GetCountLeadTransactionsWithoutWithdrawal(int leadId, DateTime startDate, CancellationTokenSource cancellationTokenSource)
        {

            var request = new RestRequest("api/Transactions/count-transaction-without-withdrawal/", Method.Get)
                .AddParameter("leadId", leadId)
                .AddParameter("startDate", startDate.ToString("s"));

            int result = 0;
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Скачивание КОЛ-ВА транзакций лида " + leadId);
                var response = await GetResponseAsync<int>(request, cancellationTokenSource);
                result = response.Data;
                Console.WriteLine("У лида " + leadId + " КОЛ-ВО транзакций " + result);
            }

            return result;
        }

        public async Task<List<TransactionResponseModel>?> GetLeadTransactionsDepositWithdrawForLastMonth(int leadId, CancellationTokenSource cancellationTokenSource)
        {

            var request = new RestRequest("api/Transactions/by-leadId-last-month/", Method.Get)
                .AddParameter("leadId", leadId);

            List<TransactionResponseModel>? result;
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("СКАЧИВАНИЕ транзакций лида с айди " + leadId);
                var response = await GetResponseAsync<List<TransactionResponseModel>>(request, cancellationTokenSource);
                result = response.Data;
                Console.WriteLine("Транзакции СКАЧАНЫ у лида с айди " + leadId + " их " + result?.Count);
            }
            else
                result = new();

            return result;
        }

    }
}