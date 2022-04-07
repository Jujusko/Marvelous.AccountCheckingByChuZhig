using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Marvelous.Producers;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public interface ILeadProducer
    {
        //Task SendLeads(ListLeadsForUpdateRole leads);
        //Task SendLeads(List<LeadForUpdateRole> leads);
        Task SendLeads(LeadForUpdateRole lead);
    }
}