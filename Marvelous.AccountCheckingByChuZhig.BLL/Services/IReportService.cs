using Marvelous.AccountCheckingByChuZhig.BLL.Models;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface IReportService
    {
        Task<List<TransactionResponseModel>?> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate);
        Task<List<LeadModel>?> NewGetAllLeads(int start, int amount);
    }
}