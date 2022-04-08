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
        private readonly IReportService _reportService;
        private const int _daysAfterBirthday = 14;
        private const int _requiredTransactionsNumberInTwoLastMonths = 42;
        private const decimal _requiredDifferenceWthdrawDeposit = 13000m;
        private DateTime _defaultStartDate = DateTime.Now.AddMonths(-2);
        public CheckerRules(IReportService reportService)
        {
            _reportService = reportService;
        }

        public bool CheckLeadBirthday(LeadForUpdateRole _lead)
        {
            int yearDifference = 2022 - _lead.BirthDate.Year;
            DateTime date = _lead.BirthDate.AddYears(yearDifference);
            var daysAfterBirthday = DateTime.Now.Subtract(date).Days;
            var result = daysAfterBirthday >= 0 && daysAfterBirthday <= _daysAfterBirthday;
            return result;
        }
        public async Task<bool> CheckCountLeadTransactionsAsync(LeadForUpdateRole _lead)
        {
            var countTransactionsWithoutWithdraw = await _reportService.GetCountLeadTransactionsWithoutWithdrawal(_lead.Id);
            var result = countTransactionsWithoutWithdraw >= _requiredTransactionsNumberInTwoLastMonths;
            return result;

        }

        public async Task<bool> CheckDifferenceWithdrawDeposit(LeadForUpdateRole _lead)
        {
            var leadTransactionsLastMonthWithdrawDeposit = await _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(_lead.Id);

            if (leadTransactionsLastMonthWithdrawDeposit is null || leadTransactionsLastMonthWithdrawDeposit.Count == 0)
            {
                return false;
            }
            decimal difference = 0;
            foreach (ShortTransactionResponse trans in leadTransactionsLastMonthWithdrawDeposit)
            {
                difference += trans.Amount * trans.Rate;
            }
            var result = difference > _requiredDifferenceWthdrawDeposit || difference < -_requiredDifferenceWthdrawDeposit;
            return result;
        }

    }
}
