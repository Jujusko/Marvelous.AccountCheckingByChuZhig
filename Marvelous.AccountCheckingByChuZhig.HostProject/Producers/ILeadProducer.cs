using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts.ExchangeModels;
using System.Collections.Concurrent;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public interface ILeadProducer
    {
        ConcurrentBag<LeadForUpdateRole> ProcessedLeads { get; set; }
        Task SendLeads(List<LeadShortExchangeModel> leads);
    }
}