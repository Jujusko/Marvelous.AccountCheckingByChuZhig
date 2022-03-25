using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Models
{
    public class MessageModel
    {
        public MessageModel(LeadModel lead)
        {
            Lead = lead;
            //Message = $"Здарова {lead.Name})) Ваш статус изменился на {lead.Role.ToString()}";
        }

        public LeadModel Lead { get; set; }
        public string Message { get; set; }

    }
}
