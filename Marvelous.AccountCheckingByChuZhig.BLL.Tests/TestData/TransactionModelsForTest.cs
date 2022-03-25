using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Tests
{
    public class TransactionModelsForTest
    {
        public AccountModelsForTests AccountModels { get; private set; }
        private readonly Random random;
        private readonly CurrencyParser _parser;
        public TransactionModelsForTest()
        {
            AccountModels = new AccountModelsForTests();
            random = new Random();
            _parser = new();
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

        public List<TransactionModel> GetRandomTransactions(int transactionsCount)
        {
            AccountModel[] leadAccounts = AccountModels.GetAccounts();
            CurrencyParser parser = new();
            var transactions = new List<TransactionModel>();

            for (int i = 1; i <= transactionsCount; i++)
            {
                int selectAccount = random.Next(0, leadAccounts.Length);
                int amount = random.Next(1, 1000);
                int typeTransaction = random.Next(1, 4);
                DateTime transactionDate = GetRandomDateTime();

                TransactionModel transaction = new TransactionModel
                {
                    Id = i,
                    Date = transactionDate,
                    AccountId = selectAccount,
                    Amount = amount,
                    Currency = leadAccounts[selectAccount].CurrencyType,
                    Type = (TransactionType)typeTransaction
                };
                transactions.Add(transaction);
                if ((TransactionType)typeTransaction == TransactionType.Transfer)
                {
                    i++;
                    int otherAccountId = GetOtherAccount(selectAccount);
                    TransactionModel transaction1 = new TransactionModel
                    {
                        Id = i,
                        Date = transactionDate,
                        AccountId = otherAccountId,
                        Amount = amount * _parser.ParseCurrencyAmountTo(leadAccounts[selectAccount].CurrencyType, leadAccounts[otherAccountId].CurrencyType, transactionDate),
                        Currency = leadAccounts[otherAccountId].CurrencyType,
                        Type = (TransactionType)typeTransaction
                    };
                    transactions.Add(transaction1);
                }
            }
            return transactions;
        }

        private DateTime GetRandomDateTime()
        {
            int year = 2022;
            int month;
            int day;
            month = random.Next(1, 4);
            if (month == 3)
                day = random.Next(1, 22);
            else
                day = random.Next(1, 29);

            int hour = random.Next(0, 24);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);
            return new DateTime(year, month, day, hour, minute, second);
        }

        private int GetOtherAccount(int accountIdFrom)
        {
            int result = random.Next(0, 2);
            if (result == 0 && accountIdFrom != 0)
            {
                return random.Next(0, accountIdFrom);
            }
            else if (result == 1 && accountIdFrom != 5)
            {
                return random.Next(accountIdFrom + 1, 5);
            }
            else if (accountIdFrom == 5)
            {
                return 0;
            }
            else
            {
                return 5;
            }
        }
    }
}
