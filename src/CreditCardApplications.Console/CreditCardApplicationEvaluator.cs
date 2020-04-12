using System;

namespace CreditCardApplications.Console
{
    public class CreditCardApplicationEvaluator
    {
        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshhold = 100_000;
        private const int LowIncomeThreshhold = 20_000;
        private readonly IFrequentFlyerNumberValidator _validator;
        public int ValidatorLookupCount { get; private set; }

        public CreditCardApplicationEvaluator(IFrequentFlyerNumberValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _validator.ValidatorLookupPerformed += ValidatorLookupPerformed;
        }

        private void ValidatorLookupPerformed(object sender, EventArgs e)
        {
            ValidatorLookupCount++;
        }

        public CreditCardApplicationDecision Evaluate(CreditCardApplication application)
        {

            if (application.GrossAnnualIncome >= LowIncomeThreshhold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }

            if (_validator.License == "EXPIRED")
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            _validator.ValidationMode = application.Age >= 30 ? ValidationMode.Detailed : ValidationMode.Quick;

            bool isValidFrequentFlyerValid;

            try
            {
                isValidFrequentFlyerValid = _validator.IsValid(application.FrequentFlyerNumber);
            }
            catch (Exception)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (!isValidFrequentFlyerValid)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.Age <= AutoReferralMaxAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.GrossAnnualIncome < LowIncomeThreshhold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }
    }
}