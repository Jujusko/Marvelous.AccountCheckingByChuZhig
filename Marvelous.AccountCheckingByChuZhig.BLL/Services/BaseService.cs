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
        protected string GetJsonResponse(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();

            using StreamReader sr = new(webResponse.GetResponseStream());

            return sr.ReadToEnd();
        }
    }
}
