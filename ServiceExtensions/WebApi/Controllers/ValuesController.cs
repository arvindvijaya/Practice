using Extensions.ApiBase.Interfaces;
using Extensions.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SampleApplication.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IPNotesService _pNotesService;
        private readonly ICallContextAccessor _callContextAccessor;
        private readonly IConfiguration _configuration;

        public ValuesController(IPNotesService pNotesService,IConfiguration configuration,
                                ICallContextAccessor callContextAccessor)
        {
            _pNotesService = pNotesService;
            _callContextAccessor = callContextAccessor;
            _configuration = configuration;
        }
         
        [AllowAnonymous]
        [HttpGet]
        public async Task<UserIdentity> Get(string LoginId)
        {
            var user = await _pNotesService.GetUserById(LoginId);

            if (user == null)
                return new UserIdentity();

            var secretKey = _configuration.GetSection("JWTSecretKey")?.Get<string>();
            byte[] key = Convert.FromBase64String(secretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name,JsonConvert.SerializeObject(user))}),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            var generatedToken = handler.WriteToken(token);
            user.UserToken = generatedToken; 

           return new UserIdentity
            {
               FirstName = user.FirstName,
               LastName  = user.LastName,
               UserEmail = user.UserEmail,
               UserLoginId = user.UserLoginId,
               UserToken = user.UserToken
            };
        } 

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<string> Get(int id)
        {
            var user = await _pNotesService.UpdateUser();

            return "value";
        } 
    }
}
