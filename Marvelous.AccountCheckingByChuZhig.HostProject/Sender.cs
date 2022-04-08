using Marvelous.AccountCheckingByChuZhig.BLL;
using Marvelous.AccountCheckingByChuZhig.BLL.Helpers;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using Marvelous.AccountCheckingByChuZhig.HostProject.Producers;
using NLog;
using Marvelous.Contracts;
using Marvelous.Contracts.ExchangeModels;
using Marvelous.Contracts.Enums;
using Marvelous.Producers;
using System.Collections.Concurrent;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class Sender : BackgroundService
    {
        private readonly ILogHelper _log;
        private readonly ILeadProducer _leadProducer;

        public Sender(ILeadProducer leadProducer, ILogHelper log)
        {
            _leadProducer = leadProducer;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                LeadForUpdateRole? lead;
                if (_leadProducer.ProcessedLeads.TryTake(out lead))
                    CheckerRole(lead);
            }
        }

        private void CheckerRole(LeadForUpdateRole lead)
        {
            if (lead.DeservesToBeVip && lead.Role == Role.Regular)
            {
                _log.DoAction($"Lead with Id = {lead.Id} got VIP status");
                _leadProducer.LeadsGotVip.Add(lead);
            }
            else if (!lead.DeservesToBeVip && lead.Role == Role.Vip)
            {
                _log.DoAction($"Lead with Id = {lead.Id} got REGULAR status");
                _leadProducer.LeadsLostVip.Add(lead);
            }
        }
    }
}