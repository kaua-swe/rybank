using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Enums;
using src.Enums.Bank;
using src.Interfaces.Bank;
using src.Model.Auth;
using src.Model.Bank;
using src.Model.Movement;
using src.Model.Trasnfer;

namespace src.Services.Bank
{
    public class BankService : IBankService
    {
        private readonly AppDbContext _db;

        public BankService(AppDbContext db)
        {
            _db = db;
        }

        public TypeWalletEnum VerifyTypeWallet(string wallet)
        {
            if (!Enum.TryParse<TypeWalletEnum> ( wallet, true, out var tipo))
            {
                throw new InvalidOperationException($"Carteira {wallet} não identificada");
            }
            return tipo;
        }

        public TypeTransferEnum VerifyTypeTransfer(string transfer)
        {
            if (!Enum.TryParse<TypeTransferEnum>( transfer, true, out var tipo))
            {
                throw new InvalidOperationException($"Não identificado o tipo {transfer}");
            }
            return tipo;
        }

        public async Task<WalletModel?> FindWalletById(Guid usuarioId)
        {
            var wallet = await _db.Wallet.FirstOrDefaultAsync(w => w.UsuarioId == usuarioId);
            return wallet;
        }

        public async Task<BalanceModel?> FindBalanceById(Guid usuarioId)
        {
            var balance = await _db.Balance.FirstOrDefaultAsync(w => w.UsuarioId == usuarioId);
            return balance;
        }

        public async Task<UserModel?> FindUserByEmail(string email)
        {
            var wallet = await _db.User.FirstOrDefaultAsync(e => e.Email == email);
            return wallet;
        }

        public async Task<object> CreateWallet(string email, string typeWallet)
        {
            var emailUser = email.Trim();
            var userAlreadyExists = await FindUserByEmail(emailUser);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para inserir as informações");
            }

            var verifyWallet = VerifyTypeWallet(typeWallet);
            string tipo = verifyWallet.ToString();

            var walletExists = await _db.Wallet.AnyAsync(w => w.UsuarioId == userAlreadyExists.Id && (w.Conta == tipo));
            if (walletExists)
            {
                throw new InvalidOperationException($"A conta já tem cadastrado conta {tipo}");
            }

            var utcNow = DateTime.UtcNow;

            var newWallet = new WalletModel
            {
                UsuarioId = userAlreadyExists.Id,
                Conta = tipo,
                CreatedAt = utcNow
            };

            _db.Wallet.Add(newWallet);

            var newBalance = new BalanceModel
            {
                UsuarioId = userAlreadyExists.Id,
                Conta = tipo,
                UpdatedAt = utcNow
            };

            _db.Balance.Add(newBalance);

            await _db.SaveChangesAsync();

            return new
            {
                newWallet.Conta,
                newBalance.Saldo,
                newWallet.CreatedAt,
            };
        }

        public async Task<object> Deposit(string email, decimal valor, string conta)
        {
            var emailUser = email.Trim();
            var userAlreadyExists = await FindUserByEmail(emailUser);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Conta informada inválida.");
            }
            if (valor <= 0)
            {
                throw new InvalidOperationException("Quantia inválida.");
            }

            var tipoConta = VerifyTypeWallet(conta);
            string tipo = tipoConta.ToString();

            var existsBalance = await _db.Balance.FirstOrDefaultAsync(u => u.UsuarioId == userAlreadyExists.Id && u.Conta == tipo);
            if (existsBalance == null)
            {
                throw new InvalidOperationException("Conta informada sem operação de depósito.");
            }

            var existsTipoConta = await _db.Wallet.AnyAsync(u => u.UsuarioId == userAlreadyExists.Id && (u.Conta == tipo));

            if (!existsTipoConta)
            {
                throw new InvalidOperationException($"Esta conta não recebe depósito em conta {tipo}");
            }
            var utcNow = DateTime.UtcNow;
            var saldoAnterior = existsBalance.Saldo;

            existsBalance.Saldo += valor;
            existsBalance.UpdatedAt = utcNow;

            await _db.SaveChangesAsync();
            return new
            {
                ValorAnterior = saldoAnterior,
                ValorAtual = existsBalance.Saldo,
                TipoConta = tipo,
                DataDeposito = existsBalance.UpdatedAt
            };
        }
        public async Task<object> Transfer(string email, string tipo, string chave, string conta, decimal valor)
        {
            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor não pode ser menor ou igual a zero.");
            }
            var emailUser = email.Trim();
            var userAlreadyExists = await FindUserByEmail(emailUser);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Conta informada inválida.");
            }

            var tipoWallet = VerifyTypeWallet(conta);
            string wallet = tipoWallet.ToString();

            var existsBalanceOrigem = await _db.Balance.FirstOrDefaultAsync(u => u.UsuarioId == userAlreadyExists.Id && (u.Conta == wallet));
            if (existsBalanceOrigem == null)
            {
                throw new InvalidOperationException("A conta informada não faz transferência.");
            }

            if (valor > existsBalanceOrigem.Saldo)
            {
                throw new InvalidOperationException("A conta informada não possui saldo.");
            }

            var tipoTransfer = VerifyTypeTransfer(tipo);
            string transfer = tipoTransfer.ToString();

            var utcNow = DateTime.UtcNow;

            if (transfer == TypeTransferEnum.PIX.ToString())
            {
                var existsChave = await _db.Pix.FirstOrDefaultAsync(p => p.Chave == chave);
                if (existsChave == null)
                {
                    throw new InvalidOperationException("Não encontrado a chave pix.");
                }
                if (existsChave.UsuarioId == userAlreadyExists.Id)
                {
                    throw new InvalidOperationException("Você não pode transferir a si mesmo.");
                }
                var existsBalanceDestino = await _db.Balance.FirstOrDefaultAsync(u => u.UsuarioId == existsChave.UsuarioId);
                if (existsBalanceDestino == null)
                {
                    throw new InvalidOperationException("A conta de destino não recebe transferência.");
                }

                existsBalanceOrigem.Saldo -= valor;
                existsBalanceOrigem.UpdatedAt = utcNow;
                existsBalanceDestino.Saldo += valor;
                existsBalanceDestino.UpdatedAt = utcNow;

                var newTransfer = new TransferModel
                {
                    OrigemUserId = userAlreadyExists.Id,
                    DestinoUserId = existsBalanceDestino.UsuarioId,
                    Valor = valor,
                    CreatedAt = utcNow,
                };

                _db.Transfer.Add(newTransfer);

                var newMovementOrigem = new MovementModel
                {
                    UsuarioId = userAlreadyExists.Id,
                    Tipo = TypeMovementEnum.TRANSENVIADA.ToString(),
                    Conta = wallet,
                    CreatedAt = utcNow
                };

                var newMovementDestino = new MovementModel
                {
                    UsuarioId = existsBalanceDestino.UsuarioId,
                    Tipo = TypeMovementEnum.TRANSRECEBIDA.ToString(),
                    Conta = existsBalanceDestino.Conta,
                    CreatedAt = utcNow
                };

                _db.Movement.Add(newMovementOrigem);
                _db.Movement.Add(newMovementDestino);

                await _db.SaveChangesAsync();

                return new
                {
                    Origem = email,
                    Destino = chave,
                    Conta = conta,
                    Valor = valor
                };
            }
            throw new InvalidOperationException($"Não identificado o tipo {tipo} de transferência.");
        }

        public async Task<object> Sacar(string email, string tipoConta, decimal valor)
        {
            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor não pode ser menor ou igual a zero.");
            }
            var userEmail = email.Trim();
            var userAlreadyExists = await FindUserByEmail(userEmail);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Conta informada inválida.");
            }

            var utcNow = DateTime.UtcNow;

            var tipo = VerifyTypeWallet(tipoConta);
            string wallet = tipo.ToString();

            var matchWallet = await _db.Balance.FirstOrDefaultAsync(u => u.UsuarioId == userAlreadyExists.Id && (u.Conta == wallet));
            if (matchWallet == null)
            {
                throw new InvalidOperationException("A conta informada não faz saque.");
            }

            if (matchWallet.Saldo < valor)
            {
                throw new InvalidOperationException("A conta informada não possui saldo.");
            }

            matchWallet.Saldo -= valor;
            matchWallet.UpdatedAt = utcNow;
            await _db.SaveChangesAsync();

            return new
            {
                Origem = email,
                Conta = wallet,
                Valor = valor
            };
            
        }
    }
}