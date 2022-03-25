using Marvelous.AccountCheckingByChuZhig.BLL.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Tests
{
    public class ChekerRulesTests
    {
        private CheckerRules _checkerRules;
        private LeadModelsForTests _leadModelsForTests;
        [SetUp]
        public void Setup()
        {
            _checkerRules = new CheckerRules();
            _leadModelsForTests = new LeadModelsForTests();
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public void CheckLeadBirthdayTest(int numberLead, bool expectedResult)
        {
            //given
            var lead = _leadModelsForTests.GetLeads(numberLead);

            //when
            //bool actual = _checkerRules.CheckLeadBirthday(lead);

            //than
           // Assert.AreEqual(actual, expectedResult);
        }
    }
}
