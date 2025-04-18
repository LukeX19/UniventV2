﻿using System.ComponentModel.DataAnnotations;

namespace Univent.App.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime <= DateTime.UtcNow)
                {
                    return new ValidationResult(ErrorMessage ?? "The date must be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
