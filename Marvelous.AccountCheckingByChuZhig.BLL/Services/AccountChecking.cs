using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class AccountChecking
    {
        private readonly string _urlToTakeAllLeads = "https://piter-education.ru:6010/api/leads";

        public List<LeadModel> GetAllLeads()
        {
            WebRequest myWebRequest = WebRequest.Create($"{_urlToTakeAllLeads}");
            WebResponse myWebResponse = myWebRequest.GetResponse();

            string text;
            using (var sr = new StreamReader(myWebResponse.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            List<LeadModel> result = JsonConvert.DeserializeObject<List<LeadModel>>(text);
            return result;
        }
    }
}