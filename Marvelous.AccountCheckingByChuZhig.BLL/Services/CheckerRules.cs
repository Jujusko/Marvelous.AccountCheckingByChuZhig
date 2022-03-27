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
        private CancellationTokenSource _cancellationTokenSource;
        public CheckerRules(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        public bool CheckLeadBirthday(LeadModel leadModel)
        {
            int yearDifference = DateTime.Now.Year - leadModel.BirthDate.Year;
            DateTime date = leadModel.BirthDate.AddYears(yearDifference);
            var result = date >= DateTime.Now.Subtract(TimeSpan.FromDays(14)); //Настройка от Алёны 14

            CancelToken(result);
            return result;
        }
        public bool CheckCountLeadTransactions(int countTransactionsWithoutWithdraw)
        {
            int requiredTransactionsNumber = 42; //Настройка от Алёны 42
            var result = countTransactionsWithoutWithdraw >= requiredTransactionsNumber;
            CancelToken(result);
            return result;

        }

        public bool CheckDifferenceWithdrawDeposit(List<TransactionResponseModel> leadTransactionsLastMonthWithdrawDeposit)
        {
            
            if (leadTransactionsLastMonthWithdrawDeposit is null || leadTransactionsLastMonthWithdrawDeposit.Count == 0)
                return false;

            decimal difference = 0;
            foreach (TransactionResponseModel trans in leadTransactionsLastMonthWithdrawDeposit)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                    break;

                difference += trans.Amount * trans.RubRate;
            }
            var result = difference > 13000 || difference < -13000; //Настройка от Алёны 13000
            CancelToken(result);
            return result;
        }

        private void CancelToken(bool result)
        {
            if (result)
                _cancellationTokenSource.Cancel(); 
            
        }
    }
}
