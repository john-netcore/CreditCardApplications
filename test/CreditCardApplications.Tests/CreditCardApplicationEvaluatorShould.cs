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

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            // Using It.Is() with predicate to check correct value.
            mockValidator.Setup(x => x.IsValid(It.Is<string>(x => (x.StartsWith("110x"))))).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication
            {
                Age = 50,
                GrossAnnualIncome = 19_999,
                FrequentFlyerNumber = "110x234892390"
            };

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, expected);
        }
    }
}
