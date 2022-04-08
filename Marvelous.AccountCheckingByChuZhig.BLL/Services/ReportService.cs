using AutoMapper;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using Marvelous.Contracts.Enums;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IMapper _mapper;
        public ReportService(ILogHelper logger, IMapper mapper)
        {
            _domain = ReportUrls.ReportDomain;
            _mapper = mapper;
        }
       // GetLeadTransactionsDepositWithdrawForLastMonth
        public async Task<List<LeadForUpdateRole>?> GetLeadsInRange(int start, int amount)
        {
            RestRequest request = new RestRequest("api/Leads/take-leads-in-range", Method.Get);

            request.AddParameter("offset", start);
            request.AddParameter("fetch", amount);
            var sres = await GetResponseAsync<List<LeadStatusUpdateResponse>>(request);

            var list = sres.Data;

            return _mapper.Map<List<LeadForUpdateRole>>(list);

        }

        public async Task<int> GetCountLeadTransactionsWithoutWithdrawal(int leadId)
        {

            var request = new RestRequest("api/Transactions/count-transaction-without-withdrawal/", Method.Get)
                .AddParameter("leadId", leadId);

            int result = 0;

            var response = await GetResponseAsync<int>(request);
            result = response.Data;

            return result;
        }

        public async Task<List<ShortTransactionResponse>?> GetLeadTransactionsDepositWithdrawForLastMonth(int leadId)
        {

            var request = new RestRequest("api/Transactions/by-leadId-last-month/", Method.Get)
                .AddParameter("leadId", leadId);

            List<ShortTransactionResponse>? result;
            var response = await GetResponseAsync<List<ShortTransactionResponse>>(request);
            result = response.Data;

            return result;
        }

        public async Task<int> GetCountOfLeadsByRole(Role role)
        {
            var request = new RestRequest("api/Leads/count-leads/", Method.Get)
                .AddParameter("role", role.ToString());
            var response = await GetResponseAsync<int>(request);
            var result = response.Data;

            return result;
        }

    }
}