using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using AutoMapper;
using System.Collections.Concurrent;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public class LeadProducer : ILeadProducer
    {
        private readonly ILogger<LeadProducer> _logger;
        private readonly IBus _bus;
        public ConcurrentBag<LeadForUpdateRole> ProcessedLeads { get; set; }

        public LeadProducer(ILogger<LeadProducer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
            ProcessedLeads = new();
        }

        public async Task SendLeads(List<LeadShortExchangeModel> leads)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            _logger.LogInformation("Try publish change leads");
            await _bus.Publish(leads.ToArray(), source.Token);
            leads.Clear();
            _logger.LogInformation("Leads published");
        }
    }
}
