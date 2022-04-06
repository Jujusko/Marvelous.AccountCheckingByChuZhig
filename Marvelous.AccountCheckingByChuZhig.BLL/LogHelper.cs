
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL
{
    public class LogHelper : ILogHelper
    {
        private readonly ILogger<LogHelper> _logger;

        public LogHelper(ILogger<LogHelper> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogInformation(name);
        }
    }
}
