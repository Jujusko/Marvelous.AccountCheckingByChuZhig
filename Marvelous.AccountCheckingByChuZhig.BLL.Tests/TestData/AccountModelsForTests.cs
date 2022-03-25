using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Tests
{
    public class AccountModelsForTests
    {
        public LeadModel Lead {get; private set;}
        public AccountModelsForTests()
        {
            Lead = new LeadModel
            {
                Id = 1,
                Name = "Лидок",
                LastName = "Сучка",
                BirthDate = DateTime.Now.AddDays(-19).AddYears(-13),
                Email = "yarrrrr@gmail.cum",
                IsBanned = false,
                Phone = "1-800-110-09-01",
                Role = Role.Regular
            };
            GetAccounts();
        }

        public AccountModel[] GetAccounts()
        {
            AccountModel[] accounts = new AccountModel[]
            {
                new AccountModel
                {
                    Id = 1,
                    Name = "Рубли",
                    Balance = 100000,
                    IsBlocked = false,
                    LockDate = null,
                    CurrencyType =Currency.RUB,
                    Lead = Lead
                },
                new AccountModel
                {
                    Id = 2,
                    Name = "Юани",
                    Balance = 100000,
                    IsBlocked = false,
                    LockDate = null,
                    CurrencyType =Currency.CNY,
                    Lead = Lead
                },
                new AccountModel
                {
                    Id = 3,
                    Name = "Швейцарские франки",
                    Balance = 100000,
                    IsBlocked = false,
                    LockDate = null,
                    CurrencyType =Currency.CHF,
                    Lead = Lead
                },
                new AccountModel
                {
                    Id = 4,
                    Name = "Украинские гривны",
                    Balance = 100000,
                    IsBlocked = false,
                    LockDate = null,
                    CurrencyType =Currency.UAH,
                    Lead = Lead
                },
                new AccountModel
                {
                    Id = 5,
                    Name = "Сингапурский доллар",
                    Balance = 100000,
                    IsBlocked = false,
                    LockDate = null,
                    CurrencyType =Currency.SGD,
                    Lead = Lead
                },
                new AccountModel
                {
                    Id = 6,
                    Name = "Чешских крон",
                    Balance = 100000,
                    IsBlocked = false,
                    LockDate = null,
                    CurrencyType =Currency.CZK,
                    Lead = Lead
                },
            };
            Lead.Accounts = accounts.ToList();
            return accounts;
        }
    }
}
