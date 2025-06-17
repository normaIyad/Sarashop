using System.ComponentModel.DataAnnotations;

namespace Sarashop.DTO
{
    public class SendCodeReq
    {
        [Required]
        public string Code { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "not matches passwords")]
        public string ConfirmPassword { get; set; }
    }
}
