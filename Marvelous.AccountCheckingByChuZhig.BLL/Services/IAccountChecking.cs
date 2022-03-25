using Marvelous.AccountCheckingByChuZhig.BLL.Models;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface IAccountChecking
    {
        Task<List<LeadModel>> NewGetAllLeads(int start, int amount);
        Task<List<LeadModel>> StartTasks(int start, int amount);
    }
}