using Marvelous.AccountCheckingByChuZhig.BLL.Extensions;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;


namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class AccountChecking
    {
        public async Task StartTasks(int start, int end)
        {
            await Task.Run(() => NewGetAllLeads(start, end));
        }

        public void NewGetAllLeads(int start, int amount)
        {
            WebRequest myWebRequest = WebRequest.Create($"https://piter-education.ru:6010/api/Leads/take-from-{start}-to-{amount}");
            WebResponse myWebResponse = myWebRequest.GetResponse();
            string text;
            var sr = new StreamReader(myWebResponse.GetResponseStream());
            text = sr.ReadToEnd();
            List<LeadModel> result = JsonConvert.DeserializeObject<List<LeadModel>>(text);
            BirthdayCheck(result);
        }

        private void BirthdayCheck(List<LeadModel> result)
        {
            var min = DateTime.Now.AddDays(-15);
            var max = DateTime.Now.AddDays(+15);
            var year = DateTime.Now.Year;

            int difference;
            foreach(var lead in result)
            {
                difference = year - lead.BirthDate.Year;
                DateTime currentLeadBirthday = lead.BirthDate.AddYears(difference);
                if (lead.BirthDate.Month == DateTime.Now.Month && lead.BirthDate.Day == DateTime.Now.Day)
                {
                    Console.WriteLine();
                    Console.WriteLine(lead.BirthDate);
                    Console.WriteLine("Happy birthday");
                    //запрос в црм, меняем роль лида
                }
            }
        }
    }
}