using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
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
        public bool CheckLeadTransactions(int countTransactionsWithoutWithdraw)
        {
            int requiredTransactionsNumber = 42;
            if (countTransactionsWithoutWithdraw >= requiredTransactionsNumber)
                return true;

            return false;
        }

        public bool CheckDifferenceWithdrawDeposit(List<TransactionResponseModel> leadTransactionsLastMonthWithdrawDeposit)
        {
            decimal difference = 0;
            foreach (TransactionResponseModel trans in leadTransactionsLastMonthWithdrawDeposit)
            {
                difference += trans.Amount * trans.RubRate;
            }

            if (difference > 13000 || difference < -13000)
                return true;

            return false;
        }
    }
}
