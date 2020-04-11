using System;
using CreditCardApplications.Console;
using Moq;
using Xunit;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { GrossAnnualIncome = 20_000 };

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, expected);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { Age = 19 };

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, expected);
        }
    }
}
