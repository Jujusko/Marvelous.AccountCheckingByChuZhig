﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Helpers
{
    public static class ReportUrls
    {
        public const string ReportDomain = "https://piter-education.ru:6010/";
        public static string GetAmountOfLeads = "api/Leads/take-from-{start}-to-{amount}";
        public static string GetLeads()
        {
            return "https://piter-education.ru:6010/api/leads";
        }

        public static string GetLeadTransactionsForPeriod(int leadId, DateTime start, DateTime end)
        {
            var startString = start.ToString("s");
            var endString = end.ToString("s");
            return $"https://piter-education.ru:6010/api/Transactions/by-lead-id/in-range?leadId={leadId}&startDate={startString}&finishDate={endString}";
        }

        
    }
}
