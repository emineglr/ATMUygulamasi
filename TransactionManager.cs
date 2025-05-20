namespace ATMUygulamasi
{
    public class TransactionManager
    {

        public List<Transaction> TransactionList { get; set; }
        public TransactionManager()
        {
            TransactionList = new List<Transaction>();
        }
        public void Add(Transaction transaction)
        {
            transaction.TransactionId = GetNewId();
            TransactionList.Add(transaction);

        }
        private int GetNewId()
        {
            if (TransactionList.Any())
            {
                int id = TransactionList.Max(a => a.TransactionId) + 1;
                return id;
            }
            else
                return 1;


        }
    }

}