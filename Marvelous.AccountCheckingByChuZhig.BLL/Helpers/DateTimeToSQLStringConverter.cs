using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Helpers
{
    public static class DateTimeToSQLStringConverter
    {
        public static string Convert(DateTime dateTime)
        {
            var date = dateTime.ToString("s");
            return date;
        }
    }
}
