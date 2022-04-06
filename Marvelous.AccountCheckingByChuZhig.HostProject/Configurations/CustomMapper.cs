using AutoMapper;
using Marvelous.AccountCheckingByChuZhig.BLL.Models;
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
        }
    }
}
