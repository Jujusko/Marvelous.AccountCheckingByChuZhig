using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface IReportService
    {
        Task<int> GetCountLeadTransactionsWithoutWithdrawal(int leadId);
        Task<int> GetCountOfLeadsByRole(Role role);
        Task<List<ShortTransactionResponse>?> GetLeadTransactionsDepositWithdrawForLastMonth(int leadId);
        Task<List<LeadForUpdateRole>?> GetLeadsInRange(int start, int amount);
    }
}