namespace CreditCardApplications.Console
{
    public interface IFrequentFlyerNumberValidator
    {
        bool IsValid(string frequentflyerNumber);

        void IsValid(string frequentflyerNumber, out bool isValid);
    }
}