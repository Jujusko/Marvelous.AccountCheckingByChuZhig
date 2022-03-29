using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Models
{
    public class TransactionResponseModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
}
