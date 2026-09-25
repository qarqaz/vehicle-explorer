using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VehicleExplorer.Api.Models.Requests;

namespace VehicleExplorer.Tests.Models
{
    public sealed class VehicleModelSearchRequestTests
    {
        [Fact]
        public void Validate_WithValidRequest_ReturnsNoErrors()
        {
            var request = new VehicleModelSearchRequest
            {
                MakeId = 448,
                Year = 2015,
                VehicleType = "Truck"
            };

            var results = Validate(request);

            Assert.Empty(results);
        }

        [Fact]
        public void Validate_WithFutureYear_ReturnsValidationError()
        {
            var request = new VehicleModelSearchRequest
            {
                MakeId = 448,
                Year = DateTime.UtcNow.Year + 1,
                VehicleType = "Truck"
            };

            var results = Validate(request);

            Assert.Contains(results, result => result.MemberNames.Contains(nameof(request.Year)));
        }

        [Fact]
        public void Validate_WithInvalidMakeId_ReturnsValidationError()
        {
            var request = new VehicleModelSearchRequest
            {
                MakeId = 0,
                Year = 2015,
                VehicleType = "Truck"
            };

            var results = Validate(request);

            Assert.Contains(results, result => result.MemberNames.Contains(nameof(request.MakeId)));
        }

        private static IReadOnlyList<ValidationResult> Validate(VehicleModelSearchRequest request)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(request, new ValidationContext(request), results, validateAllProperties: true);

            return results;
        }
    }
}
