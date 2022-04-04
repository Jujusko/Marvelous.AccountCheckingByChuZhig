using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using Marvelous.Producers;
namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public class LeadProducer : ILeadProducer
    {
        private readonly ILogger<LeadProducer> _logger;
        private readonly IBus _bus;

        public LeadProducer(ILogger<LeadProducer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task SendLeads(ListLeadsForUpdateRole leads)
        {

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            _logger.LogInformation("Try publish change leads");

            await _bus.Publish(leads, source.Token);
            _logger.LogInformation("Leads published");

        }
    }
}
