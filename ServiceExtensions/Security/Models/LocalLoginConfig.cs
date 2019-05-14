namespace Extensions.Security.Models
{
    /// <summary>
    /// information used to bypass token authentication for testing in development
    /// </summary>
    public class LocalLoginConfig
    {
        public bool Enabled { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserLoginId { get; set; }
        public string HondaGlobalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}