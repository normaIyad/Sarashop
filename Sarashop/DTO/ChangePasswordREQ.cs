using System.ComponentModel.DataAnnotations;

namespace Sarashop.DTO
{
    public class ChangePasswordREQ
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        public string ConfarmPassword { get; set; }


    }
}
