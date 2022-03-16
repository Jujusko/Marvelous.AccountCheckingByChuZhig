using Marvelous.Contracts;

namespace Marvelous.AccountCheckingByChuZhig.BLL
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Currency CurrencyType { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? LockDate { get; set; }
        public decimal? Balance { get; set; }
    }
}
