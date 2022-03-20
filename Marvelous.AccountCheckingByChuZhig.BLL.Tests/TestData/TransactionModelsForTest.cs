using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Tests
{
    public class TransactionModelsForTest
    {
        public AccountModelsForTests AccountModelsForTests { get; private set; }
        public TransactionModelsForTest()
        {
            AccountModelsForTests = new AccountModelsForTests();
        }
        public List<TransactionModel> GetTransactionsForTest()
        {
            List<TransactionModel> transactions = new List<TransactionModel>();
            TransactionModel transaction1 = new TransactionModel
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-10),
                AccountId = 1,
                Amount = 1000,
                Currency = Currency.RUB,
                Type = TransactionType.Deposit
            };
            TransactionModel transaction2 = new TransactionModel
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-10),
                AccountId = 1,
                Amount = 1000,
                Currency = Currency.RUB,
                Type = TransactionType.Withdraw
            };
            TransactionModel transaction3 = new TransactionModel
            {
                Id = 33,
                Date = new DateTime(2022, 3, 3, 10, 10, 10),
                AccountId = 1,
                Amount = 10,
                Currency = Currency.EUR,
                Type = TransactionType.Transfer
            };
            TransactionModel transaction4 = new TransactionModel
            {
                Id = 34,
                Date = new DateTime(2022, 3, 3, 10, 10, 10),
                AccountId = 2,
                Amount = 10,
                Currency = Currency.RUB,
                Type = TransactionType.Transfer
            };
            return transactions;
        }

        private List<TransactionModel> GetRandomTransactions(int transactionsCount)
        {
            var transactions = new List<TransactionModel>();
            for (int i = 0; i < transactionsCount; i++)
            {
                Random random = new Random();
                int currency = random.Next(1, 112);
                int amount = random.Next(1, 1000000);
                TransactionModel transaction = new TransactionModel
                {
                    Id = 34,
                    Date = new DateTime(2022, 3, 3, 10, 10, 10),
                    AccountId = 2,
                    Amount = 10,
                    Currency = Currency.RUB,
                    Type = TransactionType.Transfer
                };
            }
            return transactions;
        }
    }
}
