using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using rybank.Dto.Pay;
using rybank.Enums.Pay;
using rybank.estudo.Data;
using rybank.estudo.Enums;
using rybank.estudo.Interfaces;
using rybank.estudo.Models;
using rybank.Interfaces;

namespace rybank.estudo.Services
{
    public class BankService : IBankService
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;
        private readonly IPixService _pixService;

        public BankService(AppDbContext db, IAuthService authService, IPixService pixService)
        {
            _db = db;
            _authService = authService;
            _pixService = pixService;
        }

        public async Task<SaldoModel?> FindSaldoById(Guid usuarioId)
        {
            var saldo = await _db.Saldo.FirstOrDefaultAsync(s => s.UsuarioId == usuarioId);
            return saldo;
        } 

        public async Task<BoletoModel?> FindNumberBoleto(string codigos)
        {
            var boleto = await _db.Boleto.FirstOrDefaultAsync(b => b.Codigo == codigos);
            return boleto;
        }

        public async Task<BoletoModel> VerifiyStatusBoleto(BoletoModel boleto)
        {
            if (DateTime.UtcNow.Date > boleto.Maturity.Date && boleto.Status == StatusBoletoEnum.Pendente)
            {
                boleto.Status = StatusBoletoEnum.Expirado;
                await _db.SaveChangesAsync();
            }
            return boleto;
        }

        public async Task<TransferenciaModel?> FindTransferById(Guid usuarioId)
        {
            var user = await _db.Transferencias.FirstOrDefaultAsync(u => u.OrigemUserId == usuarioId);
            return user;
        }

        public async Task<object> Deposito(string email, decimal valor)
        {

            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor não poder igual ou menor que zero.");
            }
            var utcnow = DateTime.UtcNow;
            var userAlreadyExists = await _authService.FindByEmail(email);

            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("E-mail informado não encontrado.");
            }

            var saldo = await FindSaldoById(userAlreadyExists.Id);

            var deposito = new DepositModel
            {
                UsuarioId = userAlreadyExists.Id,
                Valor = valor,
                CreatedAt = utcnow
            };

            _db.Deposito.Add(deposito);

            var movimentacao = new MovimentacaoModel
            {
                UsuarioId = userAlreadyExists.Id,
                Valor = valor,
                Tipo = TipoMovimentacaoEnum.Deposito,
                CreatedAt = utcnow
            };

            _db.Movimentacao.Add(movimentacao);

            if (saldo == null)
            {
                saldo = new SaldoModel
                {
                    UsuarioId = userAlreadyExists.Id,
                    Saldo = valor,
                    UpdatedAt = utcnow
                };

                _db.Saldo.Add(saldo);

            } else
            {
                saldo.Saldo += valor;
            }
            await _db.SaveChangesAsync();
            return new
            {
                userAlreadyExists.Email,
                valorDepositado = valor,
                saldoAtual = saldo.Saldo
            };
        }

        public async Task<object> Sacar(string email, decimal valor)
        {

            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor nao pode ser igual ou menor que zero.");
            }
            var utcnow = DateTime.UtcNow;
            var userAlreadyExists = await _authService.FindByEmail(email);

            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("O e-mail informado não foi encontrado.");
            }
            var saldo = await FindSaldoById(userAlreadyExists.Id);
            if (saldo == null)
            {
                throw new InvalidOperationException("A conta informada não possui saldo.");
            }

            if (valor > saldo.Saldo)
            {
                throw new InvalidOperationException("A conta informada não possui saldo suficiente.");
            }

            saldo.Saldo -= valor;
            saldo.UpdatedAt = utcnow;

            var movimentacao = new MovimentacaoModel
            {
                UsuarioId = userAlreadyExists.Id,
                Valor = valor,
                Tipo = TipoMovimentacaoEnum.Saque,
                CreatedAt = utcnow
            };

            _db.Movimentacao.Add(movimentacao);

            await _db.SaveChangesAsync();
            return new
            {
                userAlreadyExists.Email,
                valorSacado = valor,
                saldoAtual = saldo.Saldo,
                updated = utcnow
            };
        }

        public async Task<object> Transferir(string origem, string tipo, string? destino, string? chave, decimal valor)
        {

            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor não pode ser menor ou igual a zero.");
            }

            if (origem == destino)
            {
                throw new InvalidOperationException("Nao pode transferir para si mesmo.");
            }
            var utcnow = DateTime.UtcNow;
            var existsOrigem = await _authService.FindByEmail(origem);


            if (existsOrigem == null)
            {

                throw new InvalidOperationException("Não encontrado a origem ou destino.");

            }

            if (tipo == TipoPagamento.PIX.ToString())
            {
                if (chave == null)
                {
                    throw new InvalidOperationException("Não foi informado a chave pix.");
                }
                var descobrirPix = await _pixService.FindChave(chave);
                if (descobrirPix == null)
                {
                    throw new InvalidOperationException("Não foi encontrado a chave pix.");
                }
                if (descobrirPix.UsuarioId == existsOrigem.Id)
                {
                    throw new InvalidOperationException("Não pode transferir para si mesmo.");
                }
                var saldoOrigem = await FindSaldoById(existsOrigem.Id);
                var saldoDestino = await FindSaldoById(descobrirPix.UsuarioId);
                if (saldoOrigem == null || saldoDestino == null)
                {
                    throw new InvalidOperationException("Carteira da origem ou destino não encontrada.");
                }
                if (valor > saldoOrigem.Saldo)
                {
                    throw new InvalidOperationException("A conta de origem não possui saldo suficiente.");
                }
                saldoOrigem.Saldo -= valor;
                saldoDestino.Saldo += valor;
                saldoOrigem.UpdatedAt = utcnow;
                saldoDestino.UpdatedAt = utcnow;


                var movimentacaoDestino = new MovimentacaoModel
                {

                    UsuarioId = descobrirPix.UsuarioId,
                    Valor = valor,
                    Tipo = TipoMovimentacaoEnum.TransRecebida
                };
                _db.Movimentacao.Add(movimentacaoDestino);

                var movimentacaoOrigem = new MovimentacaoModel
                {
                    UsuarioId = existsOrigem.Id,
                    Valor = valor,
                    Tipo = TipoMovimentacaoEnum.TransEnviada
                };

                _db.Movimentacao.Add(movimentacaoOrigem);

                var transferencias = new TransferenciaModel
                {
                    OrigemUserId = saldoOrigem.UsuarioId,
                    DestinoUserId = saldoDestino.UsuarioId,
                    Valor = valor,
                    CreatedAt = utcnow
                };

                _db.Transferencias.Add(transferencias);

                await _db.SaveChangesAsync();

                var emailDestino = await _authService.FindByEmailForId(descobrirPix.UsuarioId);

                return new
                {
                    origem = existsOrigem.Email,
                    destino = emailDestino!.Email,
                    newSaldoOrigem = saldoOrigem.Saldo,
                    newSaldoDestino = saldoDestino.Saldo  
                };
            }
            if (tipo == TipoPagamento.TED.ToString())
            {
                return new
                {
                    message = "Ok"
                };
            }
            throw new InvalidOperationException("Tipo de pagamento inválido.");
        }

        public async Task<object> GerarBoleto(string empresa, string devedor, decimal valor, int vencimento)
        {

            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor não pode ser menor ou igual a zero.");
            }
            var utcnow = DateTime.UtcNow;
            var user = await _authService.FindByEmail(devedor);

            if (user == null)
            {
                throw new InvalidOperationException("Não foi encontrado este usuário.");
            }

            var codigoboleto = RandomNumberGenerator.GetInt32(1000000000, 2000000000).ToString();
            var vencimentoBoleto = utcnow.AddDays(vencimento);

            var generateBoleto = new BoletoModel
            {
                Empresa = empresa,
                DevedorId = user.Id,
                ValorBoleto = valor,
                Codigo = codigoboleto,
                CreatedAt = utcnow,
                Maturity = vencimentoBoleto,
                Status = StatusBoletoEnum.Pendente
            };

            _db.Boleto.Add(generateBoleto);
            await _db.SaveChangesAsync();

            return new
            {
                generateBoleto.Empresa,
                generateBoleto.DevedorId,
                generateBoleto.Codigo,
                generateBoleto.CreatedAt,
                generateBoleto.Maturity,
                generateBoleto.Status
            };
        }

        public async Task<object> PagarBoleto(string conta, string codigo)
        {
            var utcnow = DateTime.UtcNow;
            var existsBoleto = await FindNumberBoleto(codigo);

            if (existsBoleto == null)
            {
                throw new InvalidOperationException("Não foi encontrado o numero do boleto");
            }
            var boleto = await VerifiyStatusBoleto(existsBoleto);

            if (boleto.Status == StatusBoletoEnum.Expirado)
            {
                throw new InvalidOperationException("O boleto está vencido.");
            }

            if (boleto.Status != StatusBoletoEnum.Pendente)
            {
                throw new InvalidOperationException("O boleto não está disponível para pagamento.");
            }
            
            var existsConta = await _authService.FindByEmail(conta);

            if (existsConta == null)
            {
                throw new InvalidOperationException("Não foi encontrada a conta de pagamento.");
            }

            var saldoConta = await FindSaldoById(existsConta.Id);

            if (saldoConta == null)
            {
                throw new InvalidOperationException("Não foi encontrada a conta de pagamento.");
            }

            if (saldoConta.Saldo < existsBoleto.ValorBoleto)
            {
                throw new InvalidOperationException("A conta não possui saldo suficiente.");
            }

            saldoConta.Saldo -= existsBoleto.ValorBoleto;
            saldoConta.UpdatedAt = utcnow;

            boleto.Status = StatusBoletoEnum.Pago;
            boleto.PayedAt = utcnow;

            var movimentacao = new MovimentacaoModel
            {
                UsuarioId = existsConta.Id,
                Valor = boleto.ValorBoleto,
                Tipo = TipoMovimentacaoEnum.Pagamento,
                CreatedAt = utcnow
            };

            _db.Movimentacao.Add(movimentacao);
            await _db.SaveChangesAsync();

            return new
            {
                boleto.Codigo,
                boleto.Empresa,
                boleto.ValorBoleto,
                boleto.Status,
                boleto.PayedAt,
                saldoAtual = saldoConta.Saldo
            };
        }

        public async Task<object> CancelarBoleto(string codigo)
        {
            var boleto = await FindNumberBoleto(codigo);
            var utcnow = DateTime.UtcNow;

            if (boleto == null)
            {
                throw new InvalidOperationException("Não encontrado o boleto para cancelar.");
            }

            if (boleto.Status != StatusBoletoEnum.Pendente)
            {
                throw new InvalidOperationException("Este boleto nao pode ser cancelado.");
            }

            boleto.Status = StatusBoletoEnum.Cancelado;
            boleto.UpdatedAt = utcnow;

            return new
            {
                boleto.Codigo,
                boleto.DevedorId,
                boleto.ValorBoleto,
                boleto.Status,
                boleto.PayedAt,
                boleto.CreatedAt,
                boleto.UpdatedAt
            };
        }

        public async Task<List<TransferResponseDto>> ListarTransferencias(string email)
        {
            var user = await _authService.FindByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Não encontrado o email informado.");
            }
            var existsTrans = await FindTransferById(user.Id);
            if (existsTrans == null)
            {
                throw new InvalidOperationException("Não encontrado transferências.");
            }
            var trans = await _db.Transferencias.Where(d => d.OrigemUserId == user.Id).Select(u => new TransferResponseDto
            {
                Id = u.Id,
                OrigemUserId = u.OrigemUserId,
                DestinoUserId = u.DestinoUserId,
                Valor = u.Valor,
                CreatedAt = u.CreatedAt
            }).ToListAsync();

            return trans;
        }

        public async Task<List<MovimentacaoResponseDto>> ListarMovimentacoes(string email)
        {
            var user = await _authService.FindByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Não foi encontrado o e-mail informado.");
            }
            var existsMovimentacoes = await _db.Movimentacao.Where(m => m.UsuarioId == user.Id).Select(m => new MovimentacaoResponseDto{
                Id = m.Id,
                UsuarioId = m.UsuarioId,
                Valor = m.Valor,
                Tipo = m.Tipo.ToString()
            }).ToListAsync();
            
            return existsMovimentacoes;
        }
    }
}