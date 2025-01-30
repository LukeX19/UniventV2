using System.ComponentModel.DataAnnotations;
using Univent.App.Authentication.Dtos;
using Univent.Domain.Enums;

namespace Univent.App.ValidationAttributes
{
    public class UniversityRequiredForStudents : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var registerDto = (RegisterRequestDto)validationContext.ObjectInstance;

            if (registerDto.Role == AppRole.Student && registerDto.UniversityId == null)
            {
                return new ValidationResult("Students must have a valid university.");
            }

            if (registerDto.Role == AppRole.Admin && registerDto.UniversityId != null)
            {
                return new ValidationResult("Admins should not be assigned a university.");
            }

            return ValidationResult.Success;
        }
    }
}
