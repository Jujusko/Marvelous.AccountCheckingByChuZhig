using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Helpers
{
    public static class ReportUrls
    {
        public static string GetLeads()
        {
            return "https://piter-education.ru:6010/api/leads";
        }

        public static string GetLeadTransactionsForPeriod(int leadId, DateTime start, DateTime end)
        {
            var startString = start.ToString("s");
            var endString = end.ToString("s");
            return $"https://piter-education.ru:6010/api/leads/{leadId}/transactions-for-period?startDate={startString}&finishDate={endString}";
        }

        
    }
}
