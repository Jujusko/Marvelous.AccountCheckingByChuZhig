using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class CRMService: BaseService
    {
        public CRMService()
        {
            _domain = CRMUrls.CrmDomain;
        }

        public async Task SetRole(int leadId, Role role)
        {
            var client = new RestClient("https://piter-education.ru:5050/");
            var request = new RestRequest("api/leads/{leadId}/role/{role}", Method.Put)
                .AddUrlSegment("leadId", leadId)
                .AddUrlSegment("role", (int)role);

            await client.ExecuteAsync(request);
            
        }

    }
}
