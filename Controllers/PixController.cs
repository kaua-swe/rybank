using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rybank.Dto;
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
    }
}