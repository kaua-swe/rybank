using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTOs.Auth;
using src.Interfaces.Register;

namespace src.Controllers.Auth
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {

        private readonly IRegisterService _registerService;

        public AuthController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                var email = dto.Email; // Trim já é tratado no serviço
                var senha = dto.Senha;
                var nomeCompleto = dto.NomeCompleto;
                var cpf = dto.CPF;
                var telefone = dto.Telefone;
                var conta = dto.Conta;

                var success = await _registerService.CreateUser(email, senha, nomeCompleto, cpf, telefone, conta);
                return Ok(success);
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
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            try
            {
                var email = dto.Email; // Trim já é tratado no serviço
                var senha = dto.Senha;
                var success = await _registerService.Authentication(email, senha);
                return Ok(success);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        
    }
}