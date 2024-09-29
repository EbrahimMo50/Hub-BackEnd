using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_managment_system.Models;

namespace User_managment_system.Repositories
{
    public class Auth
    {
        private readonly AppDbContext _context;
        public Auth(AppDbContext context)
        {
            _context = context;
        }
        public string GenerateToken(Models.User user)
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

        private ClaimsIdentity GenerateClaims(Models.User user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            if(user.Group != null)
            {
                claims.AddClaim(new Claim("groupId", user.Group.Id.ToString()));
                var group = _context.Groups.FirstOrDefault(g => g.Id == user.Group.Id);  
                if(group != null)
                    foreach(var permission in group.Validations)
                        claims.AddClaim(new Claim(permission + "Permission", "true"));
            }
               
            else
                claims.AddClaim(new Claim("groupId", ""));

            
            return claims;
        }
    }
}
