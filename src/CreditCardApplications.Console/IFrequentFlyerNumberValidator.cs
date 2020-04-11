namespace CreditCardApplications.Console
{
    public interface IFrequentFlyerNumberValidator
    {
        string License { get; }
        bool IsValid(string frequentflyerNumber);

        void IsValid(string frequentflyerNumber, out bool isValid);
    }
}