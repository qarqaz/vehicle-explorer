using System.Text.Json.Serialization;

namespace VehicleExplorer.Api.Models.Nhtsa
{
    public sealed class NhtsaMake
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; init; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; init; } = string.Empty;
    }
}
