using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rybank.Dto;
using rybank.Dto.Pix;
using rybank.Interfaces;

namespace rybank.Controllers
{

    [ApiController]
    [Route("pix")]
    public class PixController : ControllerBase
    {
        private readonly IPixService _pixService;

        public PixController(IPixService pixService)
        {
            _pixService = pixService;
        }

        [HttpPost("cadastrar/chave")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateChave(PixCreateDto dto)
        {
            try
            {
                var createChave = await _pixService.CreatePix(dto.Email.Trim(), dto.TipoChave, dto.ValorChave);

                return Ok(createChave);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("consultar/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> Consultar(string email)
        {
            try
            {
                var consultSuccess = await _pixService.Consultar(email);
                return Ok(consultSuccess);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("deletar/chave")]
        public async Task<IActionResult> Deletar(PixDeleteDto dto)
        {
            try
            {
                var deleteSuccess = await _pixService.Deletar(dto.Email, dto.TipoChave);
                return Ok(deleteSuccess);
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