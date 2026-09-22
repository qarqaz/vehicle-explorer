using System.Text.Json.Serialization;

namespace VehicleExplorer.Api.Models.Nhtsa
{
    public sealed class NhtsaVehicleModel
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; init; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; init; } = string.Empty;

        [JsonPropertyName("Model_ID")]
        public int ModelId { get; init; }

        [JsonPropertyName("Model_Name")]
        public string ModelName { get; init; } = string.Empty;

        public int VehicleTypeId { get; init; }

        public string VehicleTypeName { get; init; } = string.Empty;
    }
}
