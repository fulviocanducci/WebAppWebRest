using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppWebRest.Models;

namespace WebAppWebRest.Controllers
{    

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private async Task<IdentityResult> CreateUserAsync(UserManager<IdentityUser> userManager)
        {
            IdentityResult result = null;
            if (await userManager.FindByNameAsync("fulviocanducci@hotmail.com") == null)
            {
                result = await userManager.CreateAsync(new IdentityUser
                {
                    UserName = "fulviocanducci@hotmail.com",
                    Email = "fulviocanducci@hotmail.com"
                }, "12345@Ab");
            }
            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody]User user, 
            [FromServices]UserManager<IdentityUser> userManager,
            [FromServices]SignInManager<IdentityUser> signInManager,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            await CreateUserAsync(userManager);

            bool credenciaisValidas = false;
            IdentityUser userIdentity = null;

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {               
                userIdentity = await userManager.FindByNameAsync(user.Email);
                if (userIdentity != null)
                {
                    var resultadoLogin = await signInManager.CheckPasswordSignInAsync(userIdentity, user.Password, false);
                    credenciaisValidas = resultadoLogin.Succeeded;
                }
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.Email, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, userIdentity.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
                    }
                );

                DateTime NotBefore = DateTime.Now;
                DateTime Expires = NotBefore.AddSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();

                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = NotBefore,
                    Expires = Expires
                });

                var token = handler.WriteToken(securityToken);

                return Ok(new
                {
                    authenticated = true,
                    created = NotBefore.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = Expires.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                });
            }
            else
            {
                return NotFound(new
                {
                    authenticated = false,
                    message = "Ops Error Authenticable"
                });
            }
        }
    }
}

//https://medium.com/@renato.groffe/asp-net-core-2-0-jwt-identity-core-na-autentica%C3%A7%C3%A3o-de-apis-e2a6fab07421
//https://medium.com/@renato.groffe/asp-net-core-2-0-jwt-identity-core-na-autentica%C3%A7%C3%A3o-de-apis-e2a6fab07421