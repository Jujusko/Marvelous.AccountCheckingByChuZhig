using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public interface ILeadProducer
    {
        Task SendMessage(int leadId, Role role);
    }
}