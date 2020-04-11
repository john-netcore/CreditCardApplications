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
            bool isValid = true;
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { Age = 19 };

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, expected);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            bool isValid = true;
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            //Using mock setup with output parameter.
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication
            {
                Age = 50,
                GrossAnnualIncome = 19_999,
            };

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, expected);
        }

        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            bool isValid = false;
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication();

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, expected);
        }

        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("EXPIRED");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication();

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, expected);
        }

        [Fact]
        public void UseDetailedLookupForOlderApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("OK");
            // To be able to track the changes made to the property.
            mockValidator.SetupProperty(x => x.ValidationMode);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { Age = 30 };

            sut.Evaluate(crediCardApplication);

            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }

        [Fact]
        public void ShouldValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication();

            sut.Evaluate(crediCardApplication);
            bool isValid;
            mockValidator.Verify(x => x.IsValid(null, out isValid));
        }
    }
}
