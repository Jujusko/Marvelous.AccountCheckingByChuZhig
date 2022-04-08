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

        public async Task<List<LeadForUpdateRole>?> GetLeadsInRange(int startRange, int amount)
        {
            var paramNameOffset = "offset";
            var paramNameFetch = "fetch";

            RestRequest request = new RestRequest(ReportUrls.GetLeadsTakeInRange, Method.Get);
            request.AddParameter(paramNameOffset, startRange);
            request.AddParameter(paramNameFetch, amount);
            var response = await GetResponseAsync<List<LeadStatusUpdateResponse>>(request);

            var list = response.Data;

            return _mapper.Map<List<LeadForUpdateRole>>(list);

        }

        public async Task<int?> GetCountLeadTransactionsWithoutWithdrawal(int leadId)
        {
            var leadIdParamName = "leadId";
            var request = new RestRequest(ReportUrls.GetCountLeadTransactionsWithoutWithdrawl, Method.Get)
                .AddParameter(leadIdParamName, leadId);
           
            var response = await GetResponseAsync<int>(request);
            int? result = response.Data;

            return result;
        }

        public async Task<List<ShortTransactionResponse>?> GetLeadTransactionsDepositWithdrawForLastMonth(int leadId)
        {
            var leadIdParamName = "leadId";
            var request = new RestRequest(ReportUrls.GetLeadTransactionsWithdrawlAndDeposit, Method.Get)
                .AddParameter(leadIdParamName, leadId);

            List<ShortTransactionResponse>? result;
            var response = await GetResponseAsync<List<ShortTransactionResponse>>(request);
            result = response.Data;

            return result;
        }

    }
}