namespace CreditCardApplications.Console
{
    public class FraudLookup
    {

        //The method must be declared as virtual for Mock to be able to create an instance of it.
        public virtual bool IsFraudRisk(CreditCardApplication application)
        {
            if (application.LastName == "Smith")
            {
                return true;
            }

            return false;
        }
    }
}