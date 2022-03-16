using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL
{
    public class LeadResponse
    {
        [JsonInclude]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<AccountResponse> Accounts { get; set; }
        public Role Role { get; set; }
        public bool IsBanned { get; set; }
    }
}
