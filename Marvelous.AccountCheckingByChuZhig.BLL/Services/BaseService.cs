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
        protected async Task<RestResponse<T>?> GetResponseAsync<T>(RestRequest request, CancellationTokenSource cancellationTokenSource)
        {
            var client = new RestClient(_domain);
            try
            {
                return await client.ExecuteAsync<T>(request, cancellationTokenSource.Token);
            }
            catch (Exception)
            {
                return new RestResponse<T> { IsSuccessful = false };
            }
               
            



        }

    }
}
