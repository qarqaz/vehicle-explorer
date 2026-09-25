namespace VehicleExplorer.Api.Models.Responses
{
    public sealed class NhtsaResponse<T>
    {
        public int Count { get; init; }

        public string? Message { get; init; }

        public string? SearchCriteria { get; init; }

        public IReadOnlyList<T> Results { get; init; } = [];

    }
}
