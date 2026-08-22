using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTOs.Pix;
using src.Interfaces.Register;

namespace src.Controllers.Pix
{

    [ApiController]
    [Route("pix")]
    public class PixController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public PixController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("create/key")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterKeyPix(RegisterPixDto dto)
        {
            try
            {
                var email = dto.Email;
                var tipoChave = dto.TipoChave; //CPF EMAIL
                var chave = dto.Chave; // 198...
                var conta = dto.Conta; // CORRENTE POUPANCA

                var success = await _registerService.RegisterKeyPix(email, tipoChave, chave, conta);

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

        [HttpPost("delete/key")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteKey(DeletePixDto dto)
        {
            try
            {
                var email = dto.Email;
                var chave = dto.Chave;

                var success = await _registerService.DeleteKey(email, chave);

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