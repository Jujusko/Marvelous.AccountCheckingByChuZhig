// See https://aka.ms/new-console-template for more information
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.BLL.Tests;
using Marvelous.AccountCheckingByChuZhig.BLL.Worker;
using RestSharp;

ReportService ac = new();

await ac.GetLeadTransactionsForPeriod(3, DateTime.Now.AddDays(-10), DateTime.Now);

int a = 0;