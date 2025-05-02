using Sarashop.Models;
using Sarashop.Valedation;
using System.ComponentModel.DataAnnotations;

namespace Sarashop.DTO
{
    public class RegesterDTO
    {
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Over18Yearcs]
        public DateTime BirthDate { get; set; }
    }
}
