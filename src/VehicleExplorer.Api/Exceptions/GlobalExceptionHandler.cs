using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VehicleExplorer.Api.Exceptions
{
    public sealed class GlobalExceptionHandler
        : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var problemDetails = exception switch
            {
                NhtsaApiException => new ProblemDetails
                {
                    Status = StatusCodes.Status502BadGateway,
                    Title = "Vehicle data service unavailable",
                    Detail = "Unable to retrieve vehicle data from NHTSA. " +
                        "Please try again later."
                },

                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred",
                    Detail = "The server encountered an unexpected error."
                }
            };

            if (exception is NhtsaApiException)
            {
                _logger.LogWarning(
                    exception, "An error occurred while communicating with NHTSA.");
            }
            else
            {
                _logger.LogError(
                    exception, "An unhandled application error occurred.");
            }

            httpContext.Response.StatusCode =
                problemDetails.Status
                ?? StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
    }
}