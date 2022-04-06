using Marvelous.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Models
{
    public class LeadForUpdateRole
    {
        public int Id
        {
            get;
            set;
        }

        public Role Role
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public DateTime BirthDate
        {
            get;
            set;
        }
        public bool DeservesToBeVip
        {
            get;
            set;
        }
    }
}
