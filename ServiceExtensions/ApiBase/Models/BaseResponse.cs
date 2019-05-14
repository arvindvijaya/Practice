namespace Extensions.ApiBase.Models
{
    /// <summary>
    /// base response for all api calls
    /// </summary>
    public class BaseResponse
    {
        public string Message { get; set; }

        public long? Value  { get; set; }

        public decimal? Amount { get; set; }
    }
}
