using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using Marvelous.Producers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using AutoMapper;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public class LeadProducer : ILeadProducer
    {
        private readonly ILogger<LeadProducer> _logger;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private List<LeadShortExchangeModel> _leadForUpdateRoles;

        public LeadProducer(ILogger<LeadProducer> logger, IBus bus, IMapper mapper)
        {
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
            _leadForUpdateRoles = new();
        }

        public async Task SendLeads(LeadForUpdateRole lead)
        {
            var leadFOrSend = _mapper.Map<LeadShortExchangeModel>(lead);
            _leadForUpdateRoles.Add(leadFOrSend);

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            if (_leadForUpdateRoles.Count == 100)
            {
                _logger.LogInformation("Try publish change leads");
                await _bus.Publish(_leadForUpdateRoles.ToArray(), source.Token);
                _leadForUpdateRoles.RemoveRange(0,100);
                _logger.LogInformation("Leads published");
            }
            
        }
    }
}
