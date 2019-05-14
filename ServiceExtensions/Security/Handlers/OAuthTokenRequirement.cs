using Microsoft.AspNetCore.Authorization;

/// <summary>
/// custom handler for validating the jwt token
/// </summary>
namespace Extensions.Security.Handlers
{
    public class OAuthTokenRequirement : IAuthorizationRequirement
    { 
    } 
}
