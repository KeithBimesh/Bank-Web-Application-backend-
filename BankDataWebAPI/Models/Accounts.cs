namespace BankDataWebAPI.Models
{
    public class Accounts
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }

    }
}
