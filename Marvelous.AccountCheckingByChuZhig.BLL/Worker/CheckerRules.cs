using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
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
        private CurrencyParser _parser;
        public CheckerRules()
        {
            _parser = new();
        }
        public bool CheckLeadBirthday(LeadModel leadModel)
        {
            int yearDifference = DateTime.Now.Year - leadModel.BirthDate.Year;
            DateTime date = leadModel.BirthDate.AddYears(yearDifference);
            return date >= 
                DateTime.Now.Subtract(TimeSpan.FromDays(14)); //считать заранее при каждом новом запуске
        }
        public bool CheckLeadTransactions(List<TransactionModel> leadTransactions)
        {
            int requiredTransactionsNumber = 42;
            if (leadTransactions.Where(cum => cum.Type != TransactionType.Withdraw).Count() < requiredTransactionsNumber)
                return false;


            return true;
        }

        public bool CheckDifferenceWithdrawDeposit(List<TransactionModel> leadTransactionsLastMonth)
        {
            decimal difference = 0;
            var transactionsWithoutTransfer = leadTransactionsLastMonth.Where(cum=>cum.Type != TransactionType.Transfer && cum.Type!=TransactionType.Service);

            foreach (TransactionModel trans in transactionsWithoutTransfer)
            {
                var amountInRub = trans.Amount * _parser.GetRateInRub(trans.Currency, trans.Date);
                if (trans.Type ==  TransactionType.Withdraw)
                    difference -= amountInRub;
                
                else
                    difference += amountInRub;
                
            }

            if(difference > 13000 || difference < -13000)
                return true;

            return false;
        }
    }
}
