using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public class SenderService
    {
        private readonly ICheckerRules _checkerRules;
        public SenderService(ICheckerRules checkerRules)
        {
            _checkerRules = checkerRules;
        }

        public async Task StartCheck(LeadModel lead)
        {
            Task checkBirthday = new Task(() => _checkerRules.CheckLeadBirthday(lead));
            await checkBirthday;
        }
    }
}
