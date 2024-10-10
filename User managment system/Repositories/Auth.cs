using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
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
            claims.AddClaim(new Claim("password", user.Password));
            claims.AddClaim(new Claim("id", user.Id.ToString()));
            if(user.Group != null)
            {
                claims.AddClaim(new Claim("groupId", user.Group.Id.ToString()));

                var group = _context.Groups.FirstOrDefault(g => g.Id == user.Group.Id);  
                if(group != null)
                {
                    claims.AddClaim(new Claim("getPermission", group.Validations.Contains("get") ? "true" : "false"));
                    claims.AddClaim(new Claim("postPermission", group.Validations.Contains("post") ? "true" : "false"));
                    claims.AddClaim(new Claim("putPermission", group.Validations.Contains("put") ? "true" : "false"));
                    claims.AddClaim(new Claim("deletePermission", group.Validations.Contains("delete") ? "true" : "false"));
                }

                //foreach(var permission in group.Validations)
                //    claims.AddClaim(new Claim(permission + "Permission", "1"));

                //to keep the consistancy of the payload will keep the claims in all users not in the users whom have it only and add a bool to it
            }

            else
                claims.AddClaim(new Claim("groupId", ""));

            
            return claims;
        }
    }
}
