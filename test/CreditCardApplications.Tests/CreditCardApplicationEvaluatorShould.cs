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
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()));
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { Age = 19 };

            var expected = sut.Evaluate(crediCardApplication);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, expected);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

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
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()));
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
        public void ValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication();

            sut.Evaluate(crediCardApplication);
            mockValidator.Verify(x => x.IsValid(null), Times.Once);
        }

        [Fact]
        public void NotValidateFrequentFlyerNumberForHighIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { GrossAnnualIncome = 20_000 };

            sut.Evaluate(crediCardApplication);
            mockValidator.Verify(x => x.IsValid(null), Times.Never);
        }

        [Fact]
        public void CheckLicenseKeyForLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { GrossAnnualIncome = 19_000 };

            sut.Evaluate(crediCardApplication);
            mockValidator.VerifyGet(x => x.License);
        }

        [Fact]
        public void SetDetailedLookupForOlderApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.License).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var crediCardApplication = new CreditCardApplication { Age = 30 };

            sut.Evaluate(crediCardApplication);
            mockValidator.VerifySet(x => x.ValidationMode = ValidationMode.Detailed);
        }

        [Fact]
        public void ReferWhenFrequentlyFlyerValidationError()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

        }
    }
}
