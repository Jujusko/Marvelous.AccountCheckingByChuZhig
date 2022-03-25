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
        public ReportService()
        {
            _domain = ReportUrls.ReportDomain;
        }

        public async Task<List<TransactionResponseModel>?> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate)
        {
            //var client = new RestClient("https://piter-education.ru:6010/");
            var request = new RestRequest("api/Transactions/by-lead-id/in-range/", Method.Get)
                .AddParameter("leadId", leadId)
                .AddParameter("startDate", startDate.ToString("s"))
                .AddParameter("endDate", endDate.ToString("s"));

            return GetResponseAsync<List<TransactionResponseModel>>(request).Result.Data;
        }

    }
}