using Extensions.Security.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Extensions.Security
{
    /// <summary>
    /// provide a implementation to return current user from ICurrentUserAccessor
    /// </summary>
    public class HttpContextCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserIdentity GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.User?.Identities?.FirstOrDefault(x => x is UserClaimIdentity) as UserClaimIdentity;
            return user?.UserIdentity;
        }
    }
}
