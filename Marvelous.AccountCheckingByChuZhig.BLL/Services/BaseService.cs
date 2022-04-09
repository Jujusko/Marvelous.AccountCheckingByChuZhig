using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public abstract class BaseService
    {
        protected string _domain;
        protected async Task<RestResponse<T>?> GetResponseAsync<T>(RestRequest request)
        {
            var client = new RestClient(_domain);

            return await client.ExecuteAsync<T>(request);
        }

    }
}
