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
        public bool DeservesToBeVip { get; private set; }
        private const int _daysAfterBirthday = 14;
        private const int _requiredTransactionsNumberInTwoLastMonths = 42;
        private const decimal _requiredDifferenceWthdrawDeposit = 13000m;
        public CheckerRules(CancellationTokenSource cancellationTokenSource, LeadModel lead)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _model = lead;
            DeservesToBeVip = false;
        }

        public bool CheckLeadBirthday(LeadModel leadModel)
        {
            int yearDifference = DateTime.Now.Year - leadModel.BirthDate.Year;
            DateTime date = leadModel.BirthDate.AddYears(yearDifference);
            var daysAfterBirthday = DateTime.Now.Subtract(date).Days;
            var result = daysAfterBirthday >= 0 && daysAfterBirthday <= _daysAfterBirthday;

            CancelToken(result, "ДНЮХА у лида " + _model.Id );
            return result;
        }
        public bool CheckCountLeadTransactions(int countTransactionsWithoutWithdraw)
        {
            var result = countTransactionsWithoutWithdraw >= _requiredTransactionsNumberInTwoLastMonths;
            CancelToken(result, "КОЛИЧЕСТВО транзакций у лида " + _model.Id );
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
                    Console.WriteLine("Пересчёт транзакций лида " + _model.Id + "  остановлен");
                    break;
                }

                difference += trans.Amount * trans.Rate;
            }
            var result = difference > _requiredDifferenceWthdrawDeposit || difference < -_requiredDifferenceWthdrawDeposit;
            if (!result)
            {
                Console.WriteLine("МАЛАЯ суММА " + difference + " у лида " + _model.Id);
            }
            CancelToken(result, "ДОСТАТОЧНАЯ суММА " + difference + " у лида " + _model.Id );
            return result;
        }

        private void CancelToken(bool result, string message)
        {
            if (result && !_cancellationTokenSource.IsCancellationRequested)
            {
                //if (_model.Role != Role.Vip.ToString())
                //{
                // поменять роль какой-нибудь LeadSendModel, в лог записать message почему роль измнилась
                //тогда может и не нужно булевые значения в тасках проверок возвращать
                //}
                DeservesToBeVip = true;
                _cancellationTokenSource.Cancel();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Поток остановлен " + message);
                Console.ResetColor();
            }
        }
    }
}
