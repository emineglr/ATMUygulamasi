namespace ATMUygulamasi
{
    public class Fraud
    {
        public int FraudId { get; set; }
        public FraudType FraudType { get; set; } //işlem tipi 
        public string FraudDesc { get; set; } // info
        public DateTime CreateDate { get; set; }
        public User User { get; set; }

    }
}