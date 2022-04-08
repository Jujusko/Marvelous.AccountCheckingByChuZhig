using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Extensions;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class CheckerRules
    {
        private readonly IReportService _reportService;
        private const int _daysAfterBirthday = -14;
        private const int _requiredTransactionsNumberInTwoLastMonths = 42;
        private const decimal _requiredDifferenceWthdrawDeposit = 13000m;

        public CheckerRules(IReportService reportService)
        {
            _reportService = reportService;
        }

        public bool CheckLeadBirthday(LeadForUpdateRole _lead)
        {
            int yearDifference = DateTime.Now.Year - _lead.BirthDate.Year;
            DateTime date = _lead.BirthDate.AddYears(yearDifference);
            bool result = date.IsInRange(DateTime.Now.AddDays(_daysAfterBirthday),DateTime.Now);
            return result;
        }
        public bool CheckCountLeadTransactionsAsync(LeadForUpdateRole _lead)
        {
            var countTransactionsWithoutWithdraw = 
                _reportService.GetCountLeadTransactionsWithoutWithdrawal(_lead.Id).Result;
            var result = countTransactionsWithoutWithdraw >= _requiredTransactionsNumberInTwoLastMonths;
            return result;
        }

        public bool CheckDifferenceWithdrawDeposit(LeadForUpdateRole _lead)
        {
            var leadTransactionsLastMonthWithdrawDeposit = 
                _reportService.GetLeadTransactionsDepositWithdrawForLastMonth(_lead.Id).Result;
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
