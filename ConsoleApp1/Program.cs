// See https://aka.ms/new-console-template for more information
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using RestSharp;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig;

CRMService ac = new();
//CheckerRules _checkerRules = new CheckerRules();
//LeadModel lead = new();
//List<TransactionResponseModel>? transactions = await ac.GetLeadTransactionsForPeriod(3, DateTime.Now.AddDays(-10), DateTime.Now);

//Task task1 = new Task<bool>(() => _checkerRules.CheckLeadBirthday(lead));
//Task task2 = new Task<bool>(() => _checkerRules.CheckLeadTransactions(transactions));
//Task task3 = new Task<bool>(() => _checkerRules.CheckDifferenceWithdrawDeposit(transactions));

//var test = new TestAsyncs();
//Task task1 = new Task<bool>(() => test.GetRandomCycle1());
//Task task2 = new Task<bool>(() => test.GetRandomCycle2());
//Task task3 = new Task<bool>(() => test.GetRandomCycle3());

//await Task.WhenAny(task1, task2, task3);

// определяем и запускаем задачи
//var task1 = PrintAsync("Hello C#", 2000);
//var task2 = PrintAsync("Hello World", 200000);
//var task3 = PrintAsync("Hello METANIT.COM", 20000);

//// ожидаем завершения хотя бы одной задачи
//if (await task1 || await task2 || await task3)
//{
//    bool result = true;
//}

//async Task<bool> PrintAsync(string message, int maxdelay)
//{
//    await Task.Delay(new Random().Next(1000, 200000));     // имитация продолжительной 
//    Console.WriteLine(message);
//    return true;
//}

await ac.SetRole(3, Marvelous.Contracts.Enums.Role.Regular);

int a = 0;