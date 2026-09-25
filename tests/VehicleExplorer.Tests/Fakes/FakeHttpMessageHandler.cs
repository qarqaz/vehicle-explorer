using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VehicleExplorer.Tests.Fakes
{
    public sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _responseContent;

        public FakeHttpMessageHandler(string responseContent)
        {
            _responseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_responseContent)
            };

            return Task.FromResult(response);
        }
    }
}
