using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Marvelous.Producers;
using System.Collections.Concurrent;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public interface ILeadProducer
    {
        //Task SendLeads(ListLeadsForUpdateRole leads);
        //Task SendLeads(List<LeadForUpdateRole> leads);
        ConcurrentBag<LeadForUpdateRole> ProcessedLeads { get; set; }
        //List<LeadShortExchangeModel> LeadsGotVip { get; set; }
        //List<LeadShortExchangeModel> LeadsLostVip { get; set; }
        Task SendLeads(List<LeadShortExchangeModel> leads);
    }
}