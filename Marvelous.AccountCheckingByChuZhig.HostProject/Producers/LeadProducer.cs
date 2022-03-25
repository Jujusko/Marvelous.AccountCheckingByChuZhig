using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Producers
{
    public class LeadProducer : ILeadProducer
    {
        private readonly ILogger<LeadProducer> _logger;

        public LeadProducer(
            ILogger<LeadProducer> logger
            )
        {
            _logger = logger;
        }

        public async Task SendMessage(int leadId, Role role)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                //cfg.Host("rabbitmq://localhost", hst =>
                //{
                //    hst.Username("guest");
                //    hst.Password("guest");
                //});
                cfg.Host("rabbitmq://80.78.240.16", hst =>
                {
                    hst.Username("nafanya");
                    hst.Password("qwe!23");
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                await busControl.Publish<ILeadShortExchangeModel>(new
                {
                    Id = leadId,
                    Role = role
                });

            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
