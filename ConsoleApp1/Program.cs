// See https://aka.ms/new-console-template for more information
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.BLL.Tests;
using Marvelous.AccountCheckingByChuZhig.BLL.Worker;

//Console.WriteLine("Hello, World!");
//ReportService ac = new();
////var asc = ac.GetAllLeads();

//var d = ac.GetLeadTransactionsForPeriod(3, DateTime.Now.AddDays(5), DateTime.Now);

CurrencyParser currencyParser = new CurrencyParser();
TransactionModelsForTest test = new();
var result = test.GetRandomTransactions(100);

CheckerRules checker = new();
checker.CheckLeadTransactions(result.Where(cum=>cum.Date>DateTime.Now.AddMonths(-2)).ToList());

checker.CheckDifferenceWithdrawDeposit(result.Where(cum => cum.Date > DateTime.Now.AddMonths(-1)).ToList());
int a = 0;