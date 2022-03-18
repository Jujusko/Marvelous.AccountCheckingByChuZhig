using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class ReportService: BaseService
    {
        public List<LeadModel> GetAllLeads()
        {
            WebRequest myWebRequest = WebRequest.Create($"{ReportUrls.GetLeads()}");
            WebResponse myWebResponse = myWebRequest.GetResponse();

            string text;
            using (var sr = new StreamReader(myWebResponse.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            List<LeadModel> result = JsonConvert.DeserializeObject<List<LeadModel>>(text);
            return result;
        }

        public List<TransactionModel> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate)
        {
            string url = $"{ReportUrls.GetLeadTransactionsForPeriod(leadId, startDate, endDate)}";
            
            string text = GetJsonResponse(url);
            
            List<TransactionModel> result = JsonConvert.DeserializeObject<List<TransactionModel>>(text);
            return result;
        }
        
    }
}