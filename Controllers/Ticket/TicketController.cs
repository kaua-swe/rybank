using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTOs.Ticket;
using src.Interfaces.Register;

namespace src.Controllers.Ticket
{

    [ApiController]
    [Route("ticket")]
    public class TicketController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public TicketController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTicket(RegisterTicketDto dto)
        {
            try
            {
                var empresa = dto.Empresa;
                var cnpj = dto.CNPJ;
                var vencimento = dto.Vencimento;
                var valor = dto.Valor;

                var success = await _registerService.CreateTicket(empresa, cnpj, vencimento, valor);

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
        [HttpPost("cancell")]
        [AllowAnonymous]
        public async Task<IActionResult> CancellTicket(CancellTicketDto dto)
        {
            try
            {
                var codigo = dto.Codigo;

                var success = await _registerService.CancellTicket(codigo);

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

        [HttpPost("pay")]
        [AllowAnonymous]
        public async Task<IActionResult> PayTicket(PayTicketDto dto)
        {
            try
            {
                var email = dto.Email;
                var codigo = dto.Codigo;
                var conta = dto.Conta;

                var success = await _registerService.PayTicket(email, codigo, conta);

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

        [HttpGet("list/{empresa}")]
        [AllowAnonymous]
        public async Task<IActionResult> ListTicket(string empresa)
        {
            try
            {
                var success = await _registerService.ListTicket(empresa);
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