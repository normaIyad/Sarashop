using System.ComponentModel.DataAnnotations;

namespace Sarashop.Valedation
{
    //ValidationAttribute
    public class Over18Yearcs : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                if (DateTime.Now.Year - date.Year > 18)
                {
                    return true;
                }
            }
            return false;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be more than 18 years";
        }

    }
}
