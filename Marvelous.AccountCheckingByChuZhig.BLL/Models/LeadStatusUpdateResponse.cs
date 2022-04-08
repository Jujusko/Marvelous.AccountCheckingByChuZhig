using Marvelous.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Models
{
    public class LeadStatusUpdateResponse
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}
