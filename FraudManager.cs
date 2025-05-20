namespace ATMUygulamasi
{
    public class FraudManager
    {
        public List<Fraud> FraudList { get; set; }
        public FraudManager()
        {
            FraudList = new List<Fraud>();
        }
        public void Add(Fraud fraud)
        {
            FraudList.Add(fraud);
        }

    }
}