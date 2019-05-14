using Extensions.ApiBase.Interfaces;
using Extensions.CoreBase;
using Extensions.Security;
using Extensions.Security.Models;
using Microsoft.AspNetCore.Http;

namespace Extensions.ApiBase.Services
{
    /// <summary>
    /// default implementation for the the current call context accessor
    /// </summary>
    public class DefaultCallContextAccessor : ICallContextAccessor
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public DefaultCallContextAccessor(ICurrentUserAccessor currentUserAccessor,IHttpContextAccessor httpContextAccessor)
        {
            _currentUserAccessor = currentUserAccessor;
            _httpContextAccesor = httpContextAccessor;
        } 

        public UserIdentity GetCurrentUser()
        {
            return _currentUserAccessor.GetCurrentUser();
        }

        public string GetUserToken()
        {
            return _httpContextAccesor.HttpContext.Request.TryGetHeader(HeaderConstants.JwtTokenName);
        }
    }
}
