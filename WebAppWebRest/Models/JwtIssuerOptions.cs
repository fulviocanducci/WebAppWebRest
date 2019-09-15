using Microsoft.IdentityModel.Tokens;

namespace WebAppWebRest.Models
{
    public class JwtIssuerOptions
    {
        public SigningCredentials SigningCredentials;
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
