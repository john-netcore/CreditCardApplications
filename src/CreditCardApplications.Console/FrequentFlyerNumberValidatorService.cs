namespace CreditCardApplications.Console
{
    public class FrequentFlyerNumberValidatorService : IFrequentFlyerNumberValidator
    {
        public string License => throw new System.NotImplementedException("For Demo purposes");

        public ValidationMode ValidationMode
        {
            get => throw new System.NotImplementedException("For Demo purposes");
            set => throw new System.NotImplementedException("For Demo purposes");
        }

        public bool IsValid(string frequentflyerNumber)
        {
            throw new System.NotImplementedException("For Demo Purposes.");
        }

        public void IsValid(string frequentflyerNumber, out bool isValid)
        {
            throw new System.NotImplementedException("For Demo Purposes.");
        }
    }
}