using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface IReportService
    {
        Task<int?> GetCountLeadTransactionsWithoutWithdrawal(int leadId);
        Task<List<ShortTransactionResponse>?> GetLeadTransactionsDepositWithdrawForLastMonth(int leadId);
        Task<List<LeadForUpdateRole>?> GetLeadsInRange(int startRange, int amount);
    }
}