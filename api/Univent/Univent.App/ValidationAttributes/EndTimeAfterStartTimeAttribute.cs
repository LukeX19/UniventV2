using System.ComponentModel.DataAnnotations;

namespace Univent.App.ValidationAttributes
{
    public class EndTimeAfterStartTimeAttribute : ValidationAttribute
    {
        private readonly string _startTime;

        public EndTimeAfterStartTimeAttribute(string startTime)
        {
            _startTime = startTime;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTimeProperty = validationContext.ObjectType.GetProperty(_startTime);
            if (startTimeProperty == null)
            {
                return new ValidationResult($"Unknown property: {_startTime}");
            }

            var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
            var endTimeValue = value as DateTime?;

            if (startTimeValue == null || endTimeValue == null)
            {
                return ValidationResult.Success;
            }

            if (endTimeValue <= startTimeValue)
            {
                return new ValidationResult(ErrorMessage ?? "End time must be after start time.");
            }

            return ValidationResult.Success;
        }
    }
}
