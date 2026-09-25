using VehicleExplorer.Api.Models.Nhtsa;
using VehicleExplorer.Api.Models.Responses;
using System.Text.Json;
using VehicleExplorer.Api.Exceptions;

namespace VehicleExplorer.Api.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(HttpClient httpClient, ILogger<VehicleService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
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
            try
            {
                _logger.LogInformation("Sending request to NHTSA: {RequestUri}", requestUri);

                var response = await _httpClient.GetFromJsonAsync<NhtsaResponse<T>>(requestUri, cancellationToken);

                var results = response?.Results ?? [];

                _logger.LogInformation("NHTSA request completed. {ResultCount} result(s) returned.", results.Count);

                return results;
            }
            catch (TaskCanceledException exception)
                when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning(exception, "NHTSA request timed out: {RequestUri}", requestUri);

                throw new NhtsaApiException("The NHTSA request timed out.", exception);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogWarning(exception, "NHTSA HTTP request failed: {RequestUri}", requestUri);

                throw new NhtsaApiException("The NHTSA API request failed.", exception);
            }
            catch (JsonException exception)
            {
                _logger.LogWarning(exception, "Invalid JSON was returned by NHTSA: {RequestUri}", requestUri);

                throw new NhtsaApiException("NHTSA returned an invalid response.", exception);
            }
        }
    }
}
