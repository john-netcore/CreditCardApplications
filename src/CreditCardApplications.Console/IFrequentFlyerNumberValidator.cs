using System;

namespace CreditCardApplications.Console
{
    public interface IFrequentFlyerNumberValidator
    {
        string License { get; }

        public ValidationMode ValidationMode { get; set; }

        bool IsValid(string frequentflyerNumber);

        void IsValid(string frequentflyerNumber, out bool isValid);

        event EventHandler ValidatorLookupPerformed;
    }

    public enum ValidationMode
    {
        Quick,
        Detailed
    }
}