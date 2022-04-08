using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using Marvelous.Producers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using AutoMapper;
using System.Collections.Concurrent;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public class LeadProducer : ILeadProducer
    {
        private readonly ILogger<LeadProducer> _logger;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        public List<LeadShortExchangeModel> LeadsGotVip { get; set; }
        public List<LeadShortExchangeModel> LeadsLostVip { get; set; }
        public ConcurrentBag<LeadForUpdateRole> ProcessedLeads { get; set; }

        public LeadProducer(ILogger<LeadProducer> logger, IBus bus, IMapper mapper)
        {
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
            ProcessedLeads = new();
            LeadsGotVip = new();
            LeadsLostVip = new();
        }

        public async Task SendLeads(List<LeadShortExchangeModel> leads)
        { 

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            if (LeadsLostVip.Count == 100)
            {
                _logger.LogInformation("Try publish change leads");
                await _bus.Publish(LeadsLostVip.ToArray(), source.Token);
                //_leadForUpdateRoles.RemoveRange(0,100);
                _logger.LogInformation("Leads published");
            }
            
        }
    }
}
