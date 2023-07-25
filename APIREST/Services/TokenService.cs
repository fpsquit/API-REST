using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIREST.Models;
using Microsoft.IdentityModel.Tokens;

namespace APIREST.Services
{
    public class TokenService
    {
          public string GeradorToken(Usuario usuario)
          {
            var tokenHandler =  new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                     Subject = new ClaimsIdentity(new Claim[]
                     {
                        new (ClaimTypes.Name, usuario.Nome),
                        new (ClaimTypes.Role, usuario.Cargo)
                     }),
                     Expires = DateTime.UtcNow.AddHours(3),
                     SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                     
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

          }



        
    }
}