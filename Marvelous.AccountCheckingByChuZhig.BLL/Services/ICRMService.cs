using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface ICRMService
    {
        Task SetRole(int leadId, Role role);
    }
}