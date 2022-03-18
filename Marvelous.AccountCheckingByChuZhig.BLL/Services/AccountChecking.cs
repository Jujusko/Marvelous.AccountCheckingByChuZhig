using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class AccountChecking
    {
        private readonly string _urlToTakeAllLeads = "https://piter-education.ru:6010/api/leads";


        public int GetAmountOfCOntacts()
        {
            int amount;
            WebRequest myWebRequest = WebRequest.Create($"{null}");
            WebResponse myWebResponse = myWebRequest.GetResponse();
            string text;
            using (var sr = new StreamReader(myWebResponse.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            return Convert.ToInt32(text);
        }

        public void MainLoop()
        {
            //ЕСЛИ ВРЕМЯ 3:00
            //code    here
            //получаем количество лидов
            //int amountOfContacts = GetAmountOfCOntacts();
            Thread[] threads = new Thread[5];
            //создаем инту (начало с 1) 
            //далее цикл на 50 потоков -> на каждый поток вызываем хранимку получающую 200 лидов и из црмки этих же лидов (?)
            int k = 0;
            int start = 10;
            for (int i = 1; i < 50; i+=10)
            {
                Console.WriteLine("Start = " + i + "END = " + start);
                //threads[k] = new Thread(() => this.GetAllLeads(i, start));
                //start += 10;
                //threads[k].Start();
                //k++;
                //if (k == 5)
                //    break;
            }
            
            //          //в потоке вызов метода который проверяет всю информацию и по логике меняем статус 
                //увеличиваем инту на 50 х 200
            //
            //
            //
        }
        public void Print(List<LeadModel> models)
        {
            foreach(var m in models)
            {

                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} PRINTS {m.Id}") ;
                     Console.WriteLine();

            }
            Console.WriteLine("_______________________________________________________________");
            Console.WriteLine($"IDIS++++++++++++++{Thread.CurrentThread.ManagedThreadId.ToString()}"); 
        }
        public List<LeadModel> GetAllLeads(int start, int end)
        {
            WebRequest myWebRequest = WebRequest.Create($"https://piter-education.ru:6010/api/Leads/take-from-{start}-to-{end}");
            WebResponse myWebResponse = myWebRequest.GetResponse();

            string text;
            using (var sr = new StreamReader(myWebResponse.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            List<LeadModel> result = JsonConvert.DeserializeObject<List<LeadModel>>(text);
            Print(result);
            return result;
        }
    }
}