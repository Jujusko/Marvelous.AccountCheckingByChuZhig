using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Worker
{
    public class CheckerRules : ICheckerRules
    {

        public bool CheckLeadBirthday(LeadModel leadModel)
        {
            int yearDifference = DateTime.Now.Year - leadModel.BirthDate.Year;
            DateTime date = leadModel.BirthDate.AddYears(yearDifference);
            return date >=
                DateTime.Now.Subtract(TimeSpan.FromDays(14)); //считать заранее при каждом новом запуске
        }
        public bool CheckLeadTransactions(List<TransactionResponseModel> leadTransactions)
        {
            int requiredTransactionsNumber = 42;
            if (leadTransactions.Where(cum => cum.Type != TransactionType.Withdraw.ToString()).Count() < requiredTransactionsNumber)
                return false;

            return true;
        }

        public bool CheckDifferenceWithdrawDeposit(List<TransactionResponseModel> leadTransactionsLastMonth)
        {
            decimal difference = 0;
            var transactionsWithoutTransfer = leadTransactionsLastMonth.Where(cum => cum.Type != TransactionType.Transfer.ToString());

            foreach (TransactionResponseModel trans in transactionsWithoutTransfer)
            {
                if (trans.Type == TransactionType.Withdraw.ToString())
                    difference -= trans.Amount * trans.RubRate;

                else
                    difference += trans.Amount * trans.RubRate;

            }

            if (difference > 13000 || difference < -13000)
                return true;

            return false;
        }
    }
}
