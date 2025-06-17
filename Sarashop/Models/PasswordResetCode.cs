namespace Sarashop.Models
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        public ApplecationUser applecationUser { get; set; }
        public string applecationUserId { get; set; }
        public string Code { get; set; }
        public DateTime ExprationCode { get; set; }

    }
}
