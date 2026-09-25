using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleExplorer.Api.Controllers;
using VehicleExplorer.Api.Models.Nhtsa;
using VehicleExplorer.Api.Models.Responses;
using VehicleExplorer.Tests.Fakes;

namespace VehicleExplorer.Tests.Controllers
{
    public sealed class VehiclesControllerTests
    {
        [Fact]
        public async Task GetMakes_ReturnsMakesSortedByName()
        {
            var vehicleService = new FakeVehicleService
            {
                Makes =
                [
                    new NhtsaMake
                {
                    MakeId = 2,
                    MakeName = "VOLVO"
                },
                new NhtsaMake
                {
                    MakeId = 1,
                    MakeName = "AUDI"
                }
                ]
            };

            var controller = new VehiclesController(vehicleService, NullLogger<VehiclesController>.Instance);

            var result = await controller.GetMakes(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var makes = Assert.IsType<MakeResponse[]>(okResult.Value);

            Assert.Equal(2, makes.Length);

            Assert.Equal("AUDI", makes[0].Name);
            Assert.Equal("VOLVO", makes[1].Name);
        }

        [Fact]
        public async Task GetVehicleTypes_WithInvalidMakeId_ReturnsValidationProblem()
        {
            var vehicleService = new FakeVehicleService();

            var controller = new VehiclesController(vehicleService, NullLogger<VehiclesController>.Instance);

            var result = await controller.GetVehicleTypes(0, CancellationToken.None);

            var errorResult = Assert.IsType<ObjectResult>(result.Result);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(errorResult.Value);

            Assert.Contains(problemDetails.Errors.Keys, key => string.Equals(key, "makeId", StringComparison.OrdinalIgnoreCase));
        }
    }
}
