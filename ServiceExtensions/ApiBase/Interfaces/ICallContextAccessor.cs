using Extensions.Security.Models;

namespace Extensions.ApiBase.Interfaces
{
    /// <summary>
    /// current call context accessor with user information
    /// </summary>
    public interface ICallContextAccessor
    {
        UserIdentity GetCurrentUser();
        string GetUserToken();
    }
}
