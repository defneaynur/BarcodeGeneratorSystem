using Core.Config.Config;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BarcodeGeneratorSystem.Api.Services.Secure
{
    public interface IAuthProcessors
    {
        bool VerifyToken(string token);
    }

    public class AuthProcessors : IAuthProcessors
    {
        private readonly IConfigProject _configProject;

        public AuthProcessors(IConfigProject configProject)
        {
            _configProject = configProject;
        }

        /// <summary>
        /// This method run for token verify
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configProject.ApiInformations.JwtSettings.Key));

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _configProject.ApiInformations.JwtSettings.Issuer,
                    ValidAudience = _configProject.ApiInformations.JwtSettings.Audience
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
            catch (SecurityTokenException ex)
            {
                return false;
            }
        }

    }
}
