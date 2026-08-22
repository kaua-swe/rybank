using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.DTOs.Ticket;
using src.Enums.Bank;
using src.Enums.Ticket;
using src.Interfaces.Ticket;
using src.Model.Auth;
using src.Model.Bank;
using src.Model.Ticket;

namespace src.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _db;

        public TicketService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserModel?> FindUserByEmail(string email)
        {
            var user = await _db.User.FirstOrDefaultAsync(e => e.Email == email);
            return user;
        }

        public async Task<TicketModel?> FindTicketByString(string codigo)
        {
            var ticket = await _db.Ticket.FirstOrDefaultAsync(n => n.Codigo == codigo);
            return ticket;
        }

        public async Task<TicketModel?> FindTicketByEmpresa(string empresaTicket)
        {
            var empresa = await _db.Ticket.FirstOrDefaultAsync(e => e.Empresa == empresaTicket);
            return empresa;
        }

        public async Task<BalanceModel?> FindBalanceByIdFilterType(Guid usuarioId, string conta)
        {
            var balance = await _db.Balance.FirstOrDefaultAsync(b => b.UsuarioId == usuarioId && (b.Conta == conta));
            return balance;
        }

        public TypeWalletEnum VerifyTypeWallet(string tipo)
        {
            if (!Enum.TryParse<TypeWalletEnum>( tipo, true, out var wallet))
            {
                throw new InvalidOperationException($"Não identificado conta {tipo}");
            }
            return wallet;
        }

        public async Task<object> CreateTicket(string empresa, string cnpj, int vencimento, decimal valor)
        {
            if (valor <= 0)
            {
                throw new InvalidOperationException("O valor do boleto não pode ser igual ou menor que zero.");
            }
            var numberGenerator = RandomNumberGenerator.GetInt32(100000000, 2000000000).ToString();
            var utcNow = DateTime.UtcNow;
            var dateVencimento = utcNow.AddDays(vencimento);

            if (!string.IsNullOrWhiteSpace(empresa))
            {
                var newTicket = new TicketModel
                {
                    Empresa = empresa,
                    Codigo = numberGenerator,
                    CNPJ = cnpj,
                    Vencimento = dateVencimento,
                    Valor = valor,
                    Status = StatusTicketEnum.PENDENTE.ToString(),
                    CreatedAt = utcNow
                };

                _db.Ticket.Add(newTicket);
                await _db.SaveChangesAsync();

                return new
                {
                    newTicket.Empresa,
                    newTicket.Codigo,
                    newTicket.Vencimento,
                    newTicket.Valor,
                    newTicket.Status,
                    newTicket.CreatedAt
                };
            }
            throw new InvalidOperationException("Necessário informar o nome da empresa.");
        }

        public async Task<object> CancellTicket(string codigo)
        {
            var utcNow = DateTime.UtcNow;
            var existsTicket = await FindTicketByString(codigo);
            if (existsTicket == null)
            {
                throw new InvalidOperationException($"Não identificado o boleto {codigo}");
            }
            if (existsTicket.Status != StatusTicketEnum.PENDENTE.ToString())
            {
                throw new InvalidOperationException($"O boleto {codigo} não pode ser cancelado.");
            }

            existsTicket.Status = StatusTicketEnum.CANCELADO.ToString();
            existsTicket.UpdatedAt = utcNow;
            await _db.SaveChangesAsync();

            return new
            {
                existsTicket.Codigo,
                existsTicket.Status,
                existsTicket.Valor,
                existsTicket.UpdatedAt,
            };
        }

        public async Task<List<ResponseTicketDto>> ListTicket(string empresa)
        {
            var existsTicket = await FindTicketByEmpresa(empresa);
            if (existsTicket == null)
            {
                throw new InvalidOperationException($"Não localizado boletos de {empresa}");
            }

            var dados = await _db.Ticket.Where(t => t.Empresa == empresa).Select(t => new ResponseTicketDto {
                Id = t.Id,
                Empresa = t.Empresa,
                Codigo = t.Codigo,
                CNPJ = t.CNPJ,
                Vencimento = t.Vencimento,
                Valor = t.Valor,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToListAsync();

            return dados;
        }

        public async Task<object> PayTicket(string email, string codigo, string conta)
        {
            var emailUser = email.Trim();
            var utcNow = DateTime.UtcNow;
            var userAlreadyExists = await FindUserByEmail(emailUser);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para continuar a operação");
            }

            var tipoConta = VerifyTypeWallet(conta);
            string wallet = tipoConta.ToString();

            var existsBalanceOrigem = await FindBalanceByIdFilterType(userAlreadyExists.Id, wallet);

            if (existsBalanceOrigem == null)
            {
                throw new InvalidOperationException($"Não encontrado a conta {wallet} para continuar a operação");
            }

            var existsTicket = await FindTicketByString(codigo);
            if (existsTicket == null)
            {
                throw new InvalidOperationException("Não encontrado o boleto para continuar a operação");
            }
            if (existsTicket.Status != StatusTicketEnum.PENDENTE.ToString())
            {
                throw new InvalidOperationException("O boleto não está disponível para pagamento.");
            }

            if (existsTicket.Valor > existsBalanceOrigem.Saldo)
            {
                throw new InvalidOperationException("A conta não possui saldo para pagar o boleto");
            }

            existsTicket.Status = StatusTicketEnum.PAGO.ToString();
            existsTicket.UpdatedAt = utcNow;
            existsBalanceOrigem.Saldo -= existsTicket.Valor;
            existsBalanceOrigem.UpdatedAt = utcNow;
            await _db.SaveChangesAsync();

            return new
            {
                Boleto = existsTicket.Codigo,
                numberCNPJ = existsTicket.CNPJ,
                numberValor = existsTicket.Valor,
                Estado = existsTicket.Status,
                Atualizado = existsTicket.UpdatedAt
            };
            
        }
    }
}