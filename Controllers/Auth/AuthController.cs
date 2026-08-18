using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rybank.estudo.Dto;
using rybank.estudo.Interfaces;

namespace rybank.estudo.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var email = dto.Email.Trim();
                var nome = dto.Name;
                var password = dto.Senha;
    
                var user = await _authService.CreateUser(nome, email, password);
                return Created("", user);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var email = dto.Email.Trim();
                var password = dto.Senha;

                var user = await _authService.Authentication(email, password);

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }
        
    }
}