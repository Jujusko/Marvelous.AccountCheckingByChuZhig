// See https://aka.ms/new-console-template for more information
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;

Console.WriteLine("Hello, World!");
ReportService ac = new();
//var asc = ac.GetAllLeads();

var d = ac.GetLeadTransactionsForPeriod(3, DateTime.Now.AddDays(5), DateTime.Now);



int a = 0;