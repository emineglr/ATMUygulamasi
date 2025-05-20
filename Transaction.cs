namespace ATMUygulamasi
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public User User { get; set; }
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime CreateDate { get; set; }

    }

}