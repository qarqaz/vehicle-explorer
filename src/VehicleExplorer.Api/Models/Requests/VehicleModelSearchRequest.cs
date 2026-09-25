using System.ComponentModel.DataAnnotations;

namespace VehicleExplorer.Api.Models.Requests
{
    public class VehicleModelSearchRequest : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Make ID must be greater than 0.")]
        public int MakeId { get; init; }

        [Range(1980, int.MaxValue, ErrorMessage = "Year must be 1980 or later.")]
        public int Year { get; init; }

        [Required(ErrorMessage = "Vehicle type is required.")]
        [StringLength(100, ErrorMessage = "Vehicle type cannot exceed 100 characters.")]
        public string VehicleType { get; init; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            var currentYear = DateTime.UtcNow.Year;

            if (Year > currentYear)
            {
                yield return new ValidationResult(
                    $"Year cannot be later than {currentYear}.",
                    [nameof(Year)]);
            }
        }
    }
}
