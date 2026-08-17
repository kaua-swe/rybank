using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rybank.estudo.Dto;
using rybank.estudo.Interfaces;

namespace rybank.estudo.Controllers
{

    [ApiController]
    [Route("pay")]
    public class PayController : ControllerBase
    {

        private readonly IBankService _bankService;

        public PayController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPost("deposit")]
        [AllowAnonymous]
        public async Task<IActionResult> Depositar(DepositDto dto)
        {
            try
            {
                var email = dto.Email.Trim();
                var valor = dto.Valor;

                var valorDepositado = await _bankService.Deposito(email, valor);

                return Ok(valorDepositado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        
        [HttpPost("sacar")]
        [AllowAnonymous]
        public async Task<IActionResult> Sacar(DepositDto dto)
        {
            try
            {
                var email = dto.Email.Trim();
                var valor = dto.Valor;

                var valorSacado = await _bankService.Sacar(email, valor);

                return Ok(valorSacado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("transferir")]
        [AllowAnonymous]
        public async Task<IActionResult> Transferir(TransferDto dto)
        {
            try
            {
                var origemEmail = dto.Origem.Trim();
                var destinoEmail = dto.Destino.Trim();
                var valor = dto.Valor;

                var valorTransferido = await _bankService.Transferir(origemEmail, destinoEmail, valor);

                return Ok(valorTransferido);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("gerarboleto")]
        [AllowAnonymous]
        public async Task<IActionResult> GerarBoleto(GenerateBoletoDto dto)
        {
            try
            {
                var empresa = dto.Empresa;
                var devedor = dto.Devedor;
                var valorBoleto = dto.ValorBoleto;
                var dataVencimento = dto.Vencimento;

                var boletoGerado = await _bankService.GerarBoleto(empresa, devedor, valorBoleto, dataVencimento);

                return Ok(boletoGerado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("pagarboleto")]
        [AllowAnonymous]
        public async Task<IActionResult> PagarBoleto(PayBoletoDto dto)
        {
            try
            {
                var conta = dto.Conta.Trim();
                var codigo = dto.Codigo;

                var boletoPago = await _bankService.PagarBoleto(conta, codigo);

                return Ok(boletoPago);

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