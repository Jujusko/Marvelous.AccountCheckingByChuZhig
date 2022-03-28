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

        public async Task<List<TransactionResponseModel>?> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate)
        {
            Console.WriteLine("СКАЧИВАНИЕ транзакций лида с айди " + leadId);
            var request = new RestRequest("api/Transactions/by-lead-id/in-range/", Method.Get)
                .AddParameter("leadId", leadId)
                .AddParameter("startDate", startDate.ToString("s"))
                .AddParameter("endDate", endDate.ToString("s"));
            var result = await GetResponseAsync<List<TransactionResponseModel>>(request);
            Console.WriteLine("Транзакции СКАЧАНЫ у лида с айди " + leadId + " их " + result.Data.Count());
            return result.Data;
        }

        public async Task<List<LeadModel>?> NewGetAllLeads(int start, int amount)
        {
            RestClient client = new RestClient(ReportUrls.ReportDomain);
            RestRequest request = new RestRequest(ReportUrls.GetAmountOfLeads, Method.Get);

            request.AddUrlSegment("start", start);
            request.AddUrlSegment("amount", amount);
            var sres = await client.ExecuteAsync<List<LeadModel>>(request);

            _logger.DoAction($"Try to get Id from rows {start} amount {amount}");//fix it

            var list = sres.Data;


            return /*метод на обработку лида*/list;
            //return BirthdayCheck(a);
        }

        public async Task<int> GetCountLeadTransactionsWithoutWithdraw(int leadId)
        {
            Console.WriteLine("Получение РАНДОМНОГО кол-ва транзакций лида " + leadId);
            Random random = new Random();
            var countTransactions = await Task<int>.Run(() => random.Next(32, 52));
            Console.WriteLine("РАНДОМНЫХ транзакций лида " + leadId +" составляет " + countTransactions);
            return countTransactions;
        }



    }
}