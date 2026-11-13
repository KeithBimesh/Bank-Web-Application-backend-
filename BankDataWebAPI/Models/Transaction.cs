namespace BankDataWebAPI.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int? RelatedAccountID { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
