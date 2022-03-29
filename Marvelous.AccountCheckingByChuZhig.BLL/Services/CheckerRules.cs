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
        private LeadModel _model;
        public CheckerRules(CancellationTokenSource cancellationTokenSource, LeadModel lead)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _model = lead;
        }

        public bool CheckLeadBirthday(LeadModel leadModel)
        {
            int yearDifference = DateTime.Now.Year - leadModel.BirthDate.Year;
            DateTime date = leadModel.BirthDate.AddYears(yearDifference);
            var ddd = DateTime.Now.Subtract(date).Days;
            var result = ddd >= 0 && ddd<=14; //Настройка от Алёны 14

            CancelToken(result, "ДНЮХА у лида " + _model.Id /*+ " " + _model.BirthDate.ToString("D")*/);
            return result;
        }
        public bool CheckCountLeadTransactions(int countTransactionsWithoutWithdraw)
        {
            int requiredTransactionsNumber = 42; //Настройка от Алёны 42
            var result = countTransactionsWithoutWithdraw >= requiredTransactionsNumber;
            CancelToken(result, "КОЛИЧЕСТВО транзакций у лида " + _model.Id /*+ " " + _model.BirthDate.ToString("D")*/);
            return result;

        }

        public bool CheckDifferenceWithdrawDeposit(List<TransactionResponseModel>? leadTransactionsLastMonthWithdrawDeposit)
        {

            if (leadTransactionsLastMonthWithdrawDeposit is null || leadTransactionsLastMonthWithdrawDeposit.Count == 0)
            {
                Console.WriteLine("У лида " + _model.Id + " ноль транзакций");
                return false;
            }
            Console.WriteLine("У лида " + _model.Id + " транзакций для ПЕРЕСЧЁТА " + leadTransactionsLastMonthWithdrawDeposit.Count());
            decimal difference = 0;
            foreach (TransactionResponseModel trans in leadTransactionsLastMonthWithdrawDeposit)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    Console.WriteLine("Пересчёт транзакций лида " + _model.Id + /*" " + _model.BirthDate.ToString("D") +*/ "  остановлен");
                    break;
                }

                difference += trans.Amount * trans.Rate;
            }
            var result = difference > 13000 || difference < -13000; //Настройка от Алёны 13000
            if (!result)
            {
                Console.WriteLine("Маленькая СУММА " + difference + " у лида " + _model.Id);
            }
            CancelToken(result, "СУММА " + difference + " у лида " + _model.Id /*+ " " + _model.BirthDate.ToString("D")*/);
            return result;
        }

        private void CancelToken(bool result, string message)
        {
            if (result && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Поток остановлен " + message);
                Console.ResetColor();
            }
        }
    }
}
