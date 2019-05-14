using Extensions.CoreBase;
using Extensions.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Extensions.Security.Handlers
{
    /// <summary>
    /// validate the current user token for each request
    /// </summary>
    public class OAuthTokenHandler : AuthorizationHandler<OAuthTokenRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// constructor for authentication handler
        /// </summary>
        /// <param name="httpContextAccessor">current httpcontextaccessor</param>
        /// <param name="logger">instance of logger</param>
        /// <param name="configuration">instance of configuration</param>
        public OAuthTokenHandler(IHttpContextAccessor httpContextAccessor,
            ILogger<OAuthTokenHandler> logger, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// method to validate the token according to custom validation rules
        /// </summary>
        /// <param name="context">current request context</param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OAuthTokenRequirement requirement)
        {
            //if exists get jwt token from header
            var userToken = _httpContextAccessor.HttpContext.Request.TryGetHeader(HeaderConstants.JwtTokenName);
            var isValidLogin = false;
            var userInfo = new UserIdentity();

            //if local login is enabled get that info from config
            var localLoginInfo = _configuration.GetSection("LocalLogin")?.Get<LocalLoginConfig>();
            //retreive the jwt secret key
            var secretKey = _configuration.GetSection("JWTSecretKey")?.Get<string>(); 

            //if local login is enabled populate user identity object from the config
            if(localLoginInfo?.Enabled == true)
            { 
                userInfo.UserName = localLoginInfo.UserName;
                userInfo.UserLoginId = localLoginInfo.UserLoginId;
                userInfo.UserEmail = localLoginInfo.UserEmail;
                userInfo.FirstName = localLoginInfo.FirstName;
                userInfo.LastName = localLoginInfo.LastName;
                userInfo.HondaGlobalId = localLoginInfo.HondaGlobalId; 
                isValidLogin = true;
            }

            //if local login is not enabled then validate the jwt retreived from header
            if (!isValidLogin)
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); 
                byte[] securitykey = Convert.FromBase64String(secretKey);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(securitykey),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                SecurityToken securityToken;

                try
                {
                    if (tokenHandler.CanReadToken(userToken))
                    {
                        ClaimsPrincipal principal = tokenHandler.ValidateToken(userToken,
                              parameters, out securityToken);

                        var decodedToken = tokenHandler.ReadToken(userToken) as JwtSecurityToken;
                        var loginId = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;
                        if (loginId != null)
                        {
                            //if the token is validated then populate user identity object with that information and mark the login as valid
                            userInfo = JsonConvert.DeserializeObject<UserIdentity>(loginId);
                            isValidLogin = true;
                        }
                    }
                }
                catch(SecurityTokenExpiredException)
                {
                    isValidLogin = false;
                }
                catch (Exception)
                {
                    isValidLogin = false;
                }
            }
            
            //if the login is valid then authentication succeeds
            if(isValidLogin)
            {
                var userIdentity = new UserClaimIdentity("oAuth") { UserIdentity = userInfo };
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, $"{userInfo.FirstName}{userInfo.LastName}"));
                userIdentity.AddClaim(new Claim(ClaimTypes.Actor, "Need userInfo UserType"));
                context.User.AddIdentity(userIdentity);
                context.Succeed(requirement);
            }
        }
    } 
}

