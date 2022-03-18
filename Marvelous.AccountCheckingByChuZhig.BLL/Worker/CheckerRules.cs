using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Worker
{
    public class CheckerRules
    {
        public bool CheckLeadBirthday(LeadModel leadModel)
        {
            return leadModel.BirthDate >= 
                DateTime.Now.Subtract(TimeSpan.FromDays(14)); //считать заранее при каждом новом запуске
        }
        public bool CheckLeadTransactions(List<TransactionModel> leadTransactions)
        {
            int requiredTransactionsNumber = 42;
            if (leadTransactions.Where(cum => cum.Type != TransactionType.Withdraw).Count() < requiredTransactionsNumber)
                return false;


            return true;
        }
    }
}
