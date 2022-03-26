using Marvelous.AccountCheckingByChuZhig.BLL.Extensions;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System.Net;
using System.Text.Json;


namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class AccountChecking : IAccountChecking
    {
        //restsharp асинхронка меньше кода

        private readonly ILogHelper _logger;
        public AccountChecking(ILogHelper logger)
        {
            _logger = logger;
        }

        public async Task<List<LeadModel>> StartTasks(int start, int amount)
        {
            return await Task.Run(() => NewGetAllLeads(start, amount));
        }

        public async Task<List<LeadModel>> NewGetAllLeads(int start, int amount)
        {
            RestClient client = new RestClient(ReportUrls.ReportDomain);
            RestRequest request = new RestRequest(ReportUrls.GetAmountOfLeads, Method.Get);

            request.AddUrlSegment("start", start);
            request.AddUrlSegment("amount", amount);
            var sres = await client.ExecuteAsync<List<LeadModel>>(request);


            var list = sres.Data;
            

            return /*метод на обработку лида*/list;
            //return BirthdayCheck(a);
        }

        private List<LeadModel> BirthdayCheck(List<LeadModel> result)
        {
            var min = DateTime.Now.AddDays(-15);
            var max = DateTime.Now.AddDays(+15);
            var year = DateTime.Now.Year;
            List<LeadModel> changedLeads = new();
            int difference;
            foreach (var lead in result)
            {
                difference = year - lead.BirthDate.Year;
                DateTime currentLeadBirthday = lead.BirthDate.AddYears(difference);
                if (lead.BirthDate.Month == DateTime.Now.Month && lead.BirthDate.Day == DateTime.Now.Day)
                {
                    Console.WriteLine();
                    Console.WriteLine(lead.BirthDate);
                    Console.WriteLine("Happy birthday");
                    //запрос в црм, меняем роль лида
                    changedLeads.Add(lead);
                    //репортинг дайте мне список лидов с др
                }
                else
                {
                    Console.WriteLine(lead.BirthDate + " " + Thread.CurrentThread.ManagedThreadId);
                }
            }
            return changedLeads;
        }
    }
}