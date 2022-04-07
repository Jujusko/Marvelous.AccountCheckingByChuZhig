using AutoMapper;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using Marvelous.Contracts.ExchangeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.HostProject.Configurations
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {
            CreateMap<LeadModel, LeadForUpdateRole>();
            CreateMap<LeadForUpdateRole, LeadShortExchangeModel>();
        }
    }
}
