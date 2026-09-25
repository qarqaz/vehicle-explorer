namespace VehicleExplorer.Api.Models.Responses
{
    public class VehicleModelResponse
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public int MakeId { get; init; }

        public string MakeName { get; init; } = string.Empty;

        public int VehicleTypeId { get; init; }

        public string VehicleTypeName { get; init; } = string.Empty;
    }
}
