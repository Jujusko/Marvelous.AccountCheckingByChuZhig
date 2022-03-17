// See https://aka.ms/new-console-template for more information
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;

Console.WriteLine("Hello, World!");
AccountChecking ac = new();
var asc = ac.GetAllLeads();
int a = 0;

 var str = ReportURLs.GetLeadTransactionForPeriod(3, DateTime.Now, DateTime.Now.AddDays(4));
