using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Interfaces.Account;
using src.Model.Account;
using src.Model.Auth;

namespace src.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;

        public AccountService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AccountModel?> FindAccountById(Guid usuarioId)
        {
            var account = await _db.Account.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);

            return account;
        }

        public async Task<UserModel?> FindUserByString(string email)
        {
            var user = await _db.User.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<object> CreateAccount(string email, string nomeCompleto, string cpf, string telefone)
        {
            var userAlreadyExists = await FindUserByString(email);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para inserir as informações.");
            }

            var existsCpf = await _db.Account.FirstOrDefaultAsync(c => c.CPF == cpf);
            if (existsCpf != null)
            {
                throw new InvalidOperationException($"O documento {cpf} não pode ser utilizado.");
            }

            var utcNow = DateTime.UtcNow;

            var numberGenerator = RandomNumberGenerator.GetInt32(1000000000, 2000000000).ToString();

            var newAccount = new AccountModel
            {
                UsuarioId = userAlreadyExists.Id,
                NomeCompleto = nomeCompleto,
                CPF = cpf,
                Telefone = telefone,
                NumeroConta = numberGenerator,
                CreatedAt = utcNow,
                UpdatedAt = utcNow
            };

            _db.Account.Add(newAccount);
            await _db.SaveChangesAsync();

            return new
            {
                newAccount.NomeCompleto,
                newAccount.CPF,
                newAccount.Telefone,
                newAccount.CreatedAt
            };
        }

        public async Task<object> UpdateAccount(string email, string nomeCompleto, string cpf, string telefone)
        {
            var userEmail = email.Trim();
            var userAlreadyExists = await FindUserByString(userEmail);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para inserir as informações.");
            }

            var existsAccount = await FindAccountById(userAlreadyExists.Id);
            if (existsAccount == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para atualizar as informações.");
            }

            existsAccount.NomeCompleto = nomeCompleto;
            existsAccount.CPF = cpf;
            existsAccount.Telefone = telefone;
            await _db.SaveChangesAsync();

            return new
            {
                existsAccount.NomeCompleto,
                existsAccount.CPF,
                existsAccount.Telefone
            };
        }
    }
}