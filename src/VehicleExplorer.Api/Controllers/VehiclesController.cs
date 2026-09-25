using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Api.Models.Responses;
using VehicleExplorer.Api.Services;
using VehicleExplorer.Api.Models.Requests;

namespace VehicleExplorer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService vehicleService, ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<VehicleTypeResponse>>> GetVehicleTypes(int makeId, CancellationToken cancellationToken)
        {
            if (makeId <= 0)
            {
                ModelState.AddModelError(nameof(makeId), "Make ID must be greater than 0.");

                return ValidationProblem(ModelState);
            }

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
        [ProducesResponseType(typeof(IReadOnlyList<VehicleModelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<VehicleModelResponse>>> GetModels([FromQuery] VehicleModelSearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching vehicle models. MakeId: {MakeId}, Year: {Year}, VehicleType: {VehicleType}", request.MakeId, request.Year, request.VehicleType);

            var models = await _vehicleService.GetModelsAsync(request.MakeId, request.Year, request.VehicleType, cancellationToken);

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
