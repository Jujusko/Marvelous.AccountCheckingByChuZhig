using Marvelous.Contracts.Enums;
using RestSharp;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public interface IConfigAlyona
    {
        Task<RestResponse<T>> SendRequest<T>(string path, Microservice service, string jwtToken = "null");
    }
}