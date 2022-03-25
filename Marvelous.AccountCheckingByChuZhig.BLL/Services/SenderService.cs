using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class SenderService
    {
        private readonly ICheckerRules _checkerRules;
        public SenderService(ICheckerRules checkerRules)
        {
            _checkerRules = checkerRules;
            Task[] tasks = new Task[]
            {

            };
        }

        public async Task<bool> StartCheckLeadBirthday(LeadModel lead)
        {
            Task<bool> checkBirthday = new Task<bool>(() => _checkerRules.CheckLeadBirthday(lead));
            await checkBirthday;
            return checkBirthday.Result;
        }

        public async Task<bool> StartCheckCountLeadTransactions(List<TransactionResponseModel> transactions)
        {
            Task<bool> checkTransactionsCount = new Task<bool>(() => _checkerRules.CheckLeadTransactions(transactions));
            await checkTransactionsCount;
            return checkTransactionsCount.Result;
        }
    }
}
