using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Tests
{
    public class LeadModelsForTests
    {
        public LeadModel GetLeads(int numberLead)
        {
           return numberLead switch
            {
                1 => new LeadModel
                {
                    Id = 1,
                    Name = "Victor",
                    LastName = "Pinos",
                    BirthDate = DateTime.Now.AddDays(-10).AddYears(-20),
                },
                2 => new LeadModel
                {
                    Id = 2,
                    Name = "Vasya",
                    LastName = "Volk",
                    BirthDate = DateTime.Now.AddDays(-20).AddYears(-20),
                },
                3 => new LeadModel
                {
                    Id = 3,
                    Name = "Varfalamey",
                    LastName = "Innokentievich",
                    BirthDate = DateTime.Now.AddDays(0).AddYears(-20),
                },
                4 => new LeadModel
                {
                    Id = 4,
                    Name = "Cat",
                    LastName = "Dog",
                    BirthDate = DateTime.Now.AddDays(-15).AddYears(-20),
                },
                5 => new LeadModel
                {
                    Id = 5,
                    Name = "Vitalya",
                    LastName = "Gorilla",
                    BirthDate = DateTime.Now.AddDays(-13).AddYears(-20),
                },
                _ => new LeadModel(),
            };
        }
    }
}
