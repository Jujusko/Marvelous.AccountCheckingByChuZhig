// See https://aka.ms/new-console-template for more information
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;

Console.WriteLine("Hello, World!");
ReportService ac = new();
var asc = ac.GetAllLeads();

//var w =Task.Run(() => first.GetJsonAsync<List<LeadModel>>("https://piter-education.ru:6010/api/Leads/take-from-1-to-20"));
//Task.WaitAny(w);
//RestResponse rrr = new();
//rrr.ResponseUri("https://piter-education.ru:6010/api/Leads/take-from-1-to-20");
//var r = w.Result;

// IRequestHelper _requestHelper;
//var response = await _requestHelper
//    .SendRequest<TransactionRequestModel>(_url, UrlTransaction.Deposit, Method.Post, transactionModel);


var client = new RestClient("https://piter-education.ru:6010/");
var request = new RestRequest("api/Leads/take-from-{qw}-to-{wq}", Method.Get);
request.AddUrlSegment("qw", 1);
request.AddUrlSegment("wq", 20);
var queryResult = JsonConvert.DeserializeObject<List<LeadModel>>(client.ExecuteAsync<List<LeadModel>>(request).Result.Content);
int a = 0;
foreach(var ase in queryResult)
{
    Console.WriteLine(ase.Email);
    Console.WriteLine();
}