namespace BankDataWebAPI.Models
{
    public class UserResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PicturePath { get; set; }
        public bool IsActive { get; set; }
    }
}
