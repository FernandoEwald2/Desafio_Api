using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System;
using Desafio.Domain.Usuarios;

namespace Desafio.Util
{
    public static class Jwt
    {
        public static string GenerateToken(Usuario usuario, string secretKey) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescritor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim("Id", usuario.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature )

            };

            var token = tokenHandler.CreateToken(tokenDescritor);

            return tokenHandler.WriteToken(token);

        }
    }
}
