using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace WebApiKalum.Helpers
{
    public class AnioAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            if(!Information.IsNumeric(value.ToString()))
            {
                return new ValidationResult("El ciclo/anio no contiene la nomenclatura adecuada");
            }
            return ValidationResult.Success;
        } 
    }
}