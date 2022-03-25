using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class ReportService : BaseService
    {
        private readonly ILogHelper _logger;
        public ReportService(ILogHelper logger)
        {
            _domain = ReportUrls.ReportDomain;
            _logger = logger;
        }

        public async Task<List<TransactionResponseModel>?> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate)
        {
            var request = new RestRequest("api/Transactions/by-lead-id/in-range/", Method.Get)
                .AddParameter("leadId", leadId)
                .AddParameter("startDate", startDate.ToString("s"))
                .AddParameter("endDate", endDate.ToString("s"));
            var result = await GetResponseAsync<List<TransactionResponseModel>>(request);
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

    }
}