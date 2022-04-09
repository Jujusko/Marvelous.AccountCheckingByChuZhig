using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Models
{
    public class LeadForUpdateRole: LeadShortExchangeModel
    {
        public DateTime BirthDate { get; set; }
        public bool DeservesToBeVip { get; set; }
        
    }
}
