using CsvHelper;
using Events.Domain.Auth;
using Events.Domain.Auth.Interfaces;
using Events.Domain.Models;
using Events.Domain.Models.JWT;
using Events.Domain.Models.RestAPI;
using Events.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> logger;
        private readonly IJwtAuthentication jwtAuthentication;

        public AuthController(ILogger<AuthController> logger, IJwtAuthentication jwtAuthentication)
        {
            this.logger = logger;
            this.jwtAuthentication = jwtAuthentication;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            var result = new Response<LoginResult>()
            {
                Succeeded = true,
                Message = "the query operation was successful",
                ErrorCode = string.Empty,
            };

            try
            {
                string requestTime = DateTime.Now.ToString("hh:mm:ss");
                Claim[] claims()
                {
                    List<Claim> claims = new()
                    {
                        new Claim(ClaimTypes.Name, request.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, request.ClientID),
                    };
                    return claims.ToArray();
                }

                // TODO: add some logic for auth 
                if(request.UserName == "admin" && request.Password == "admin")
                {
                    var jwtResult = jwtAuthentication.GenerateTokens(request.UserName, claims(), DateTime.Now);
                    result.Data = new LoginResult
                    {
                        UserName = request.UserName,
                        AccessToken = jwtResult?.AccessToken!,
                        RefreshToken = jwtResult?.RefreshToken?.TokenString
                    };

                    return Ok(result);
                }

                return Unauthorized();


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}