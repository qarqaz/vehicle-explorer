using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleExplorer.Api.Services;
using VehicleExplorer.Tests.Fakes;

namespace VehicleExplorer.Tests.Services
{
    public sealed class VehicleServiceTests
    {
        [Fact]
        public async Task GetMakesAsync_ReturnsResultsFromNhtsaResponse()
        {
            const string json = """
        {
            "Count": 2,
            "Message": "Results returned successfully",
            "SearchCriteria": null,
            "Results": [
                {
                    "Make_ID": 448,
                    "Make_Name": "TOYOTA"
                },
                {
                    "Make_ID": 474,
                    "Make_Name": "HONDA"
                }
            ]
        }
        """;

            var handler = new FakeHttpMessageHandler(json);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://vpic.nhtsa.dot.gov/api/")
            };

            var service = new VehicleService(httpClient, NullLogger<VehicleService>.Instance);

            var result = await service.GetMakesAsync();

            Assert.Equal(2, result.Count);

            Assert.Equal(448, result[0].MakeId);
            Assert.Equal("TOYOTA", result[0].MakeName);

            Assert.Equal(474, result[1].MakeId);
            Assert.Equal("HONDA", result[1].MakeName);
        }
    }
}
