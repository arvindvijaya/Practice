using Extensions.Security.Models;

namespace Extensions.Security
{
    /// <summary>
    /// Interface to get current user information
    /// </summary>
    public interface ICurrentUserAccessor
    {
        UserIdentity GetCurrentUser();
    }
}
