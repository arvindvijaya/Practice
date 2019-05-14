using System.Security.Claims;

namespace Extensions.Security.Models
{
    /// <summary>
    /// class to call base for authentication
    /// </summary>
    public class UserClaimIdentity :ClaimsIdentity
    {
       public UserIdentity UserIdentity { get; set; }

        public UserClaimIdentity(string authenticationType) : base(authenticationType)
        {

        }
    }

    /// <summary>
    /// user identity model that holds the current logged in user information
    /// </summary>
    public class UserIdentity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserLoginId { get; set; }
        public string HondaGlobalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserToken { get; set; }
    }
}
