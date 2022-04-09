using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Helpers
{
    public static class ReportUrls
    {
        public const string ReportDomain = "https://piter-education.ru:6010/";
        public const string GetLeadsTakeInRange = "api/Leads/take-leads-in-range";
        public const string GetCountLeadTransactionsWithoutWithdrawl = "api/Transactions/count-transaction-without-withdrawal/";

        public const string GetLeadTransactionsWithdrawlAndDeposit = "api/Transactions/by-leadId-last-month/";
    }
}
