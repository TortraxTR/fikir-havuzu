using api.Interfaces;
using api.Models;
using api.Dtos.Auth;
using api.Mappers.AuthMappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto loginRequest)
        {
            var user = await _userRepository
                .GetUserByPhoneNumberAsync(loginRequest.PhoneNumber);
        
            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Telefon numarası veya şifre hatalı.");

            } else if (user.IsActive == false)
            
            {
                return Unauthorized("Kullanıcı hesabı aktif değil. Lütfen yöneticinizle iletişime geçin.");
            }
        
            return Ok(user.ToLoginResponseDto());
        }
    }
}