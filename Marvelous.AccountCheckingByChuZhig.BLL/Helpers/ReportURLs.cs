﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Helpers
{
    public static class ReportURLs
    {
        public static string GetLeads()
        {
            return "https://piter-education.ru:6010/api/leads";
        }

        public static string GetLeadTransactionForPeriod(int leadId, DateTime start, DateTime end)
        {
            var startString = DateTimeToSQLStringConverter.Convert(start);
            var endString = DateTimeToSQLStringConverter.Convert(end);
            var str = $"https://piter-education.ru:6010/api/leads/{leadId}/transactions-for-period?startDate={startString}&finishDate={endString}";
            return str;
        }

        
    }
}
