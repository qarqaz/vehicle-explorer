using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Api.Models;
using VehicleExplorer.Api.Models.Responses;
using VehicleExplorer.Api.Services;

namespace VehicleExplorer.Api.Controllers
{
    [Route("api/makes")]
    [ApiController]
    public class MakesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public MakesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
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

        [HttpGet("{makeId:int}/vehicle-types")]
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
    }
}
