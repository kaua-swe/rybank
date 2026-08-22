using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTOs.Account;
using src.Interfaces.Register;

namespace src.Controllers.Account
{

    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public AccountController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAccount(UpdateAccountDto dto)
        {
            try
            {
                var email = dto.Email;
                var nomeCompleto = dto.NomeCompleto;
                var cpf = dto.CPF;
                var telefone = dto.Telefone;

                var success = await _registerService.UpdateUser(email, nomeCompleto, cpf, telefone);

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