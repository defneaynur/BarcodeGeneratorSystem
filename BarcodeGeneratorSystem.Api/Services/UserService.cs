using BarcodeGeneratorSystem.Api.Services.Base;
using BarcodeGeneratorSystem.Api.Services.Processor;
using BarcodeGeneratorSystem.Api.Services.Secure;
using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using BarcodeGeneratorSystem.Domain.Models.RequestModel;
using Core.Config.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

namespace BarcodeGeneratorSystem.Api.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserService(IUserProcessors _usersProcessors, IConfigProject _configProject) : ControllerBase
    {
        [HttpGet]
        public async Task<CoreResponse<IEnumerable<Users>>> GetUsers()
        {
            var result = await _usersProcessors.GetUsersAsync();
            if (!result.Any())
            {
                return new CoreResponse<IEnumerable<Users>>
                {
                    Data = null,
                    CoreResponseCode = CoreResponseCode.NoData,
                    ErrorMessages = new List<string>(),
                    Message = "Aradığınız kriterlerde data bulunamadı."
                };
            }

            return new CoreResponse<IEnumerable<Users>>
            {
                Data = result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }

        [HttpPost("register")]
        public async Task<CoreResponse<Users>> RegisterUser(Users user)
        {
            user.Password = Utility.HashPassword(user.Password);
            var result = await _usersProcessors.RegisterUserAsync(user);

            return new CoreResponse<Users>
            {
                Data = result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "Kullanıcı başarıyla oluşturuldu."
            };
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<CoreResponse<Users>> UpdateUserAsync(long id, [FromBody] Users user)
        {
            user.Id = id;

            var result = _usersProcessors.UpdateUserAsync(user);

            return new CoreResponse<Users>
            {
                Data = result.Result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "Ürün başarıyla güncellendi."
            };
        }


        [Authorize]
        [HttpPut("delete/{id}")]
        public async Task<CoreResponse<bool>> DeleteUserAsync(long id)
        {
            var result = await _usersProcessors.DeleteUserAsync(id);

            return new CoreResponse<bool>
            {
                Data = result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "Ürün başarıyla silindi."
            };
        }

        [HttpPost("login")]
        public async Task<CoreResponse<string>> Login(LoginRequest login)
        {
            var user = await _usersProcessors.GetUserWithUserNameAsync(login.UserName);
            if (user == null || !Utility.VerifyPassword(login.Password, user.Password))
            {
                return new CoreResponse<string>
                {
                    Data = null,
                    CoreResponseCode = CoreResponseCode.Unauthorized,
                    ErrorMessages = new List<string> { "Geçersiz kullanıcı adı veya şifre." },
                    Message = "Giriş başarısız."
                };
            }
            var jwtSettings = _configProject.ApiInformations.JwtSettings;
            var token = Security.GenerateJwtToken(user, jwtSettings);
            return new CoreResponse<string>
            {
                Data = token,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "Giriş başarılı."
            };
        }
    }
}
