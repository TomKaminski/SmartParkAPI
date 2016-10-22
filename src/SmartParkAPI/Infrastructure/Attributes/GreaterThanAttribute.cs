using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Infrastructure.Attributes
{
    public sealed class IsDateAfterAttribute : ValidationAttribute
    {
        private readonly string _testedPropertyName;
        private readonly bool _allowEqualDates;

        public IsDateAfterAttribute(string testedPropertyName, bool allowEqualDates = false)
        {
            _testedPropertyName = testedPropertyName;
            _allowEqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(_testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult($"unknown property {_testedPropertyName}");
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            // Compare values
            if ((_allowEqualDates && value == propertyTestedValue) || (DateTime)value > (DateTime)propertyTestedValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
