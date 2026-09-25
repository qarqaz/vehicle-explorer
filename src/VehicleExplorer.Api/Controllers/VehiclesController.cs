using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Api.Models.Responses;
using VehicleExplorer.Api.Services;

namespace VehicleExplorer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("makes")]
        [ProducesResponseType(typeof(IReadOnlyList<MakeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<MakeResponse>>> GetMakes(CancellationToken cancellationToken)
        {
            var makes = await _vehicleService.GetMakesAsync(cancellationToken);

            var response = makes.Select(make => new MakeResponse
            {
                Id = make.MakeId,
                Name = make.MakeName
            })
                .OrderBy(make => make.Name)
                .ToArray();

            return Ok(response);
        }

        [HttpGet("makes/{makeId:int}/vehicle-types")]
        [ProducesResponseType(typeof(IReadOnlyList<VehicleTypeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<VehicleTypeResponse>>> GetVehicleTypes(int makeId, CancellationToken cancellationToken)
        {
            var vehicleTypes = await _vehicleService.GetVehicleTypesAsync(makeId, cancellationToken);

            var response = vehicleTypes
                .Select(vehicleType => new VehicleTypeResponse
                {
                    Id = vehicleType.VehicleTypeId,
                    Name = vehicleType.VehicleTypeName
                })
                .OrderBy(vehicleType => vehicleType.Name)
                .ToArray();

            return Ok(response);
        }

        [HttpGet("models")]
        public async Task<ActionResult<IReadOnlyList<VehicleModelResponse>>> GetModels(int makeId, int year, string vehicleType, CancellationToken cancellationToken)
        {
            var models = await _vehicleService.GetModelsAsync(makeId, year, vehicleType, cancellationToken);

            var response = models
                .Select(model => new VehicleModelResponse
                {
                    Id = model.ModelId,
                    Name = model.ModelName,
                    MakeId = model.MakeId,
                    MakeName = model.MakeName,
                    VehicleTypeId = model.VehicleTypeId,
                    VehicleTypeName = model.VehicleTypeName
                })
                .OrderBy(model => model.Name)
                .ToArray();

            return Ok(response);
        }
    }
}
