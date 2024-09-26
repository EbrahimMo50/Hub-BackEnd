using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_managment_system.Models;

namespace User_managment_system.Repositories
{
    public class Auth
    {
        public string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var PrivateKey = config.GetSection("PrivateKey").Value;

            //uses private key to start the token
            var key = Encoding.ASCII.GetBytes(PrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            //describes the attributes inside the payload 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            if(user.Group != null)
                claims.AddClaim(new Claim("groupId", user.Group.Id.ToString()));
            else
                claims.AddClaim(new Claim("groupId", ""));
            return claims;
        }
    }
}
