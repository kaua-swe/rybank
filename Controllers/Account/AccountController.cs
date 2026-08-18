using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rybank.Dto.Account;
using rybank.Interfaces.Account;

namespace rybank.Controllers.Account
{

    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("dados/atualizar")]
        [AllowAnonymous]
        public async Task<IActionResult> AtualizarDados(UpdateDadosDto dto)
        {
            try
            {
                var dadosAtualizados = await _accountService.AtualizarDados(dto.Email.Trim(), dto.DisplayName, dto.CPF, dto.PhoneNumber);
                return Ok(dadosAtualizados);

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