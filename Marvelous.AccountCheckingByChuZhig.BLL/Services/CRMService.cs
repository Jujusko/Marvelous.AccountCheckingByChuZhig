using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvelous.Contracts.Enums;
using RestSharp.Authenticators;
using Marvelous.Contracts.ExchangeModels;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class CRMService : BaseService, ICRMService
    {
        private RestClient _client;
        public CRMService()
        {
            _domain = CRMUrls.CrmDomain;
            _client = new RestClient(_domain);
            _client.Authenticator = new HttpBasicAuthenticator("test", "test");
        }

        public async Task SetRole(int leadId, Role role)
        {
            var request = new RestRequest("api/leads/{leadId}/role/{role}", Method.Put)
                .AddUrlSegment("leadId", leadId)
                .AddUrlSegment("role", (int)role);

            await _client.ExecuteAsync(request);

        }

    }
}
