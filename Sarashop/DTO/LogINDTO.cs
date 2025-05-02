using System.ComponentModel.DataAnnotations;

namespace Sarashop.DTO
{
    public class LogINDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
