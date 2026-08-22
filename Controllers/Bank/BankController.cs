using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTOs.Bank;
using src.Interfaces.Register;

namespace src.Controllers.Bank
{

    [ApiController]
    [Route("bank")]
    public class BankController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public BankController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("wallet/create")]
        [AllowAnonymous]
        public async Task<IActionResult> WalletCreate(RegisterWalletDto dto)
        {
            try
            {
                var email = dto.Email;
                var conta = dto.Conta;
                var success = await _registerService.CreateWallet(email, conta);

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

        [HttpPost("wallet/deposit")]
        [AllowAnonymous]
        public async Task<IActionResult> Deposit(DepositBalanceDto dto)
        {
            try
            {
                var email = dto.Email;
                var conta = dto.Conta;
                var valor = dto.Valor;
                var success = await _registerService.Deposit(email, valor, conta);

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

        [HttpPost("wallet/saque")]
        [AllowAnonymous]
        public async Task<IActionResult> Sacar(SacarBalanceDto dto)
        {
            try
            {
                var email = dto.Email;
                var conta = dto.Conta;
                var valor = dto.Valor;
                var success = await _registerService.Sacar(email, conta, valor);

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

        [HttpPost("wallet/transfer")]
        [AllowAnonymous]
        public async Task<IActionResult> Transfer(TransferBalanceDto dto)
        {
            try
            {
                var email = dto.Email;
                var tipo = dto.Tipo;
                var chave = dto.Chave;
                var conta = dto.Conta;
                var valor = dto.Valor;

                var success = await _registerService.Transfer(email, tipo, chave, conta, valor);

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