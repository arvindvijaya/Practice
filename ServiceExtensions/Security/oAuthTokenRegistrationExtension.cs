using Extensions.Security.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Security
{
    /// <summary>
    /// extension method to register authentication policy and implementation
    /// </summary>
    public static class oAuthTokenRegistrationExtension  
    {
       public static void AddoAuthTokenSecurity(this IServiceCollection services,
           string policyName,IConfiguration config)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(policyName, policy => policy.Requirements.Add(new OAuthTokenRequirement()));
                });
            services.AddSingleton<IAuthorizationHandler, OAuthTokenHandler>(); 
        }
    }
}
