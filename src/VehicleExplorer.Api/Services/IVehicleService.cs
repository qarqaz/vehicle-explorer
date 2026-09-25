using VehicleExplorer.Api.Models.Nhtsa;

namespace VehicleExplorer.Api.Services
{
    public interface IVehicleService
    {
        Task<IReadOnlyList<NhtsaMake>> GetMakesAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NhtsaVehicleType>> GetVehicleTypesAsync(int makeId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NhtsaVehicleModel>> GetModelsAsync(int makeId, int modelYear, string vehicleType, CancellationToken cancellationToken = default);
    }
}
