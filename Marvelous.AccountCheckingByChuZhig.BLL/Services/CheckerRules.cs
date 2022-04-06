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
        //private CancellationTokenSource _cancellationTokenSource;
        //private LeadForUpdateRole _lead;
        //public bool DeservesToBeVip { get; private set; }
        private const int _daysAfterBirthday = 14;
        private const int _requiredTransactionsNumberInTwoLastMonths = 42;
        private const decimal _requiredDifferenceWthdrawDeposit = 13000m;
        public CheckerRules(/*CancellationTokenSource cancellationTokenSource, LeadForUpdateRole lead*/)
        {
            //_cancellationTokenSource = cancellationTokenSource;
            //_lead = lead;
            //_lead.DeservesToBeVip = false;
        }

        public void CheckLeadBirthday(LeadForUpdateRole _lead, CancellationTokenSource cancellationTokenSource)
        {
            int yearDifference = DateTime.Now.Year - _lead.BirthDate.Year;
            DateTime date = _lead.BirthDate.AddYears(yearDifference);
            var daysAfterBirthday = DateTime.Now.Subtract(date).Days;
            var result = daysAfterBirthday >= 0 && daysAfterBirthday <= _daysAfterBirthday;

            CancelToken(_lead, result, "ДНЮХА у лида " + _lead.Id, cancellationTokenSource);
        }
        public void CheckCountLeadTransactions(LeadForUpdateRole _lead, int countTransactionsWithoutWithdraw, CancellationTokenSource cancellationTokenSource)
        {
            var result = countTransactionsWithoutWithdraw >= _requiredTransactionsNumberInTwoLastMonths;
            CancelToken(_lead, result, "КОЛИЧЕСТВО транзакций у лида " + _lead.Id, cancellationTokenSource);

        }

        public void CheckDifferenceWithdrawDeposit(LeadForUpdateRole _lead, List<TransactionResponseModel>? leadTransactionsLastMonthWithdrawDeposit, CancellationTokenSource cancellationTokenSource)
        {

            if (leadTransactionsLastMonthWithdrawDeposit is null || leadTransactionsLastMonthWithdrawDeposit.Count == 0)
            {
                Console.WriteLine("У лида " + _lead.Id + " ноль транзакций");
                return;
            }
            Console.WriteLine("У лида " + _lead.Id + " транзакций для ПЕРЕСЧЁТА " + leadTransactionsLastMonthWithdrawDeposit.Count());
            decimal difference = 0;
            foreach (TransactionResponseModel trans in leadTransactionsLastMonthWithdrawDeposit)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    Console.WriteLine("Пересчёт транзакций лида " + _lead.Id + "  остановлен");
                    break;
                }

                difference += trans.Amount * trans.Rate;
            }
            var result = difference > _requiredDifferenceWthdrawDeposit || difference < -_requiredDifferenceWthdrawDeposit;

            CancelToken(_lead, result, "ДОСТАТОЧНАЯ суММА " + difference + " у лида " + _lead.Id, cancellationTokenSource);
        }

        private void CancelToken(LeadForUpdateRole _lead, bool result, string message, CancellationTokenSource cancellationTokenSource)
        {
            if (result && !cancellationTokenSource.IsCancellationRequested)
            {
                _lead.DeservesToBeVip = true;
                cancellationTokenSource.Cancel();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Поток остановлен " + message);
                Console.ResetColor();
            }
        }
    }
}
