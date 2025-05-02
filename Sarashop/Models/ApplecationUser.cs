using Microsoft.AspNetCore.Identity;

namespace Sarashop.Models
{
    public enum Gender
    {
        Male, Female
    }
    public class ApplecationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
