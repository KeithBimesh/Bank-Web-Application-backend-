namespace BankDataWebAPI.Models
{
    public class TransferData
    {
        public int SenderAccountID { get; set; }
        public int RecieverAccountID { get; set; }
        public decimal Amount { get; set; }
    }
}
