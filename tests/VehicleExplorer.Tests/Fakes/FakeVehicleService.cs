using System;
using System.Collections.Generic;
using System.Text;
using VehicleExplorer.Api.Models.Nhtsa;
using VehicleExplorer.Api.Services;

namespace VehicleExplorer.Tests.Fakes
{
    public sealed class FakeVehicleService : IVehicleService
    {
        public IReadOnlyList<NhtsaMake> Makes { get; set; } = [];

        public IReadOnlyList<NhtsaVehicleType> VehicleTypes { get; set; } = [];

        public IReadOnlyList<NhtsaVehicleModel> Models { get; set; } = [];

        public Task<IReadOnlyList<NhtsaMake>> GetMakesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Makes);
        }

        public Task<IReadOnlyList<NhtsaVehicleType>> GetVehicleTypesAsync(int makeId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(VehicleTypes);
        }

        public Task<IReadOnlyList<NhtsaVehicleModel>> GetModelsAsync(int makeId, int modelYear, string vehicleType, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Models);
        }
    }
}
