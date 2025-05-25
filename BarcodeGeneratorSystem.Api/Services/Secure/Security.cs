using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using Core.Config.Config.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarcodeGeneratorSystem.Api.Services.Secure
{
    public static class Security
    {
        /// <summary>
        /// This method run for create jwt token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jwtSettings"></param>
        /// <returns></returns>
        internal static string GenerateJwtToken(Users user, JwtSettings jwtSettings)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpiresInMinutes),
                signingCredentials: creds
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
