using VehicleExplorer.Api.Models.Nhtsa;
using VehicleExplorer.Api.Models.Responses;

namespace VehicleExplorer.Api.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;

        public VehicleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IReadOnlyList<NhtsaMake>> GetMakesAsync(CancellationToken cancellationToken = default)
        {
            return GetResultsAsync<NhtsaMake>("vehicles/GetAllMakes?format=json", cancellationToken);
        }

        public Task<IReadOnlyList<NhtsaVehicleType>> GetVehicleTypesAsync(int makeId, CancellationToken cancellationToken = default)
        {
            var requestUri = $"vehicles/GetVehicleTypesForMakeId/{makeId}?format=json";
            return GetResultsAsync<NhtsaVehicleType>(requestUri, cancellationToken);
        }

        public Task<IReadOnlyList<NhtsaVehicleModel>> GetModelsAsync(int makeId, int modelYear, string vehicleType, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(vehicleType);

            var encodedVehicleType = Uri.EscapeDataString(vehicleType.Trim()); //vehicle types aren't necessarily single words must use the EscapeDataString to encode it
            var requestUri =
                    $"vehicles/GetModelsForMakeIdYear/" +
                    $"makeId/{makeId}/" +
                    $"modelyear/{modelYear}/" +
                    $"vehicletype/{encodedVehicleType}" +
                    "?format=json";
            return GetResultsAsync<NhtsaVehicleModel>(requestUri, cancellationToken);
        }

        private async Task<IReadOnlyList<T>> GetResultsAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<NhtsaResponse<T>>(requestUri, cancellationToken);
            return response?.Results ?? [];
        }
    }
}
