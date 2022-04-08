using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using NLog;
using Marvelous.Contracts;
using Marvelous.Contracts.ExchangeModels;
using Marvelous.Contracts.Enums;
using System.Collections.Concurrent;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Sender : BackgroundService
    {
        private readonly ILogHelper _log;
        private readonly ILeadProducer _leadProducer;
        private const int _sizePack = 100;
        private List<LeadShortExchangeModel> LeadsForUpdate { get; set; }
        public Sender(ILeadProducer leadProducer, ILogHelper log)
        {
            _leadProducer = leadProducer;
            _log = log;
            LeadsForUpdate = new();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                LeadForUpdateRole? lead;
                if (_leadProducer.ProcessedLeads.TryTake(out lead))
                    await Task.Run(() => CheckerRole(lead));
            }
        }

        private async Task CheckerRole(LeadForUpdateRole lead)
        {
            if (lead.DeservesToBeVip && lead.Role == Role.Regular)
            {
                lead.Role = Role.Vip;
                _log.DoAction($"Lead with Id = {lead.Id} got VIP status");
                LeadsForUpdate.Add(lead);
            }
            else if (!lead.DeservesToBeVip && lead.Role == Role.Vip)
            {
                lead.Role = Role.Regular;
                _log.DoAction($"Lead with Id = {lead.Id} got REGULAR status");
                LeadsForUpdate.Add(lead);
            }
            if (LeadsForUpdate.Count == _sizePack)
            {
                await _leadProducer.SendLeads(LeadsForUpdate);
            }
        }
    }
}