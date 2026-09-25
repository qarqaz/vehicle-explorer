namespace VehicleExplorer.Api.Exceptions
{
    public class NhtsaApiException : Exception
    {
        public NhtsaApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}