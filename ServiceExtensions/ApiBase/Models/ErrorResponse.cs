namespace Extensions.ApiBase.Models
{
    /// <summary>
    /// base error response for all api calls
    /// </summary>
    public class ErrorResponse
    {
        public string Code { get; set; }

        public string Message  { get; set; } 
    }
}
