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
    public class CheckerRules
    {
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IReportService _reportService;
        private const int _daysAfterBirthday = 14;
        private const int _requiredTransactionsNumberInTwoLastMonths = 42;
        private const decimal _requiredDifferenceWthdrawDeposit = 13000m;
        private DateTime _defaultStartDate = DateTime.Now.AddMonths(-2);
        public CheckerRules(IReportService reportService, CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _reportService = reportService;
        }

        public void CheckLeadBirthday(LeadForUpdateRole _lead)
        {
            int yearDifference = 2022 - _lead.BirthDate.Year;
            DateTime date = _lead.BirthDate.AddYears(yearDifference);
            var daysAfterBirthday = DateTime.Now.Subtract(date).Days;
            var result = daysAfterBirthday >= 0 && daysAfterBirthday <= _daysAfterBirthday;

            CancelToken(_lead, result, "ДНЮХА у лида " + _lead.Id);
        }
        public async Task CheckCountLeadTransactionsAsync(LeadForUpdateRole _lead)
        {
            var countTransactionsWithoutWithdraw = await _reportService.GetCountLeadTransactionsWithoutWithdrawal(_lead.Id, _defaultStartDate, _cancellationTokenSource);
            var result = countTransactionsWithoutWithdraw >= _requiredTransactionsNumberInTwoLastMonths;
            CancelToken(_lead, result, "КОЛИЧЕСТВО транзакций у лида " + _lead.Id);

        }

        public async Task CheckDifferenceWithdrawDeposit(LeadForUpdateRole _lead)
        {
            var leadTransactionsLastMonthWithdrawDeposit = await _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(_lead.Id, _cancellationTokenSource);

            if (leadTransactionsLastMonthWithdrawDeposit is null || leadTransactionsLastMonthWithdrawDeposit.Count == 0)
            {
                //Console.WriteLine("У лида " + _lead.Id + " ноль транзакций");
                return;
            }
            //Console.WriteLine("У лида " + _lead.Id + " транзакций для ПЕРЕСЧЁТА " + leadTransactionsLastMonthWithdrawDeposit.Count());
            decimal difference = 0;
            foreach (TransactionResponseModel trans in leadTransactionsLastMonthWithdrawDeposit)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    //Console.WriteLine("Пересчёт транзакций лида " + _lead.Id + "  остановлен");
                    break;
                }

                difference += trans.Amount * trans.Rate;
            }
            var result = difference > _requiredDifferenceWthdrawDeposit || difference < -_requiredDifferenceWthdrawDeposit;

            CancelToken(_lead, result, "ДОСТАТОЧНАЯ суММА " + difference + " у лида " + _lead.Id);
        }

        private void CancelToken(LeadForUpdateRole _lead, bool result, string message)
        {
            if (result && !_cancellationTokenSource.IsCancellationRequested)
            {
                _lead.DeservesToBeVip = true;
                _cancellationTokenSource.Cancel();
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine("Поток остановлен " + message);
                //Console.ResetColor();
            }
        }
    }
}
