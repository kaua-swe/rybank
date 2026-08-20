using Microsoft.EntityFrameworkCore;
using rybank.Dto.Account;
using rybank.estudo.Data;
using rybank.estudo.Interfaces;
using rybank.Interfaces.Account;
using rybank.Models.Account;

namespace rybank.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;


        public AccountService(AppDbContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public async Task<AccountModel?> FindDadosById(Guid usuarioId)
        {
            var user = await _db.Dados.FirstOrDefaultAsync(dados => dados.UsuarioId == usuarioId);
            return user;
        }

        public async Task<object> AtualizarDados(string email, string? displayname, string? cpf, string? phonenumber)
        {
            var user = await _authService.FindByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Não encontrada a conta informada.");
            }

            if (cpf != null || phonenumber != null)
            {
                var existsCpf = await _db.Dados.FirstOrDefaultAsync(d => d.CPF == cpf);
                var existsNumber = await _db.Dados.FirstOrDefaultAsync(n => n.PhoneNumber == phonenumber);
                if (existsCpf != null || existsNumber != null)
                {
                    throw new InvalidOperationException("Já está sendo utilizado estes dados.");
                }
            }

            var existsDados = await FindDadosById(user.Id);

            if (existsDados == null)
            {
                var newDados = new AccountModel
                {
                    UsuarioId = user.Id,
                    DisplayName = displayname,
                    CPF = cpf,
                    PhoneNumber = phonenumber
                };
                _db.Dados.Add(newDados);   
            } else
            {
                if (!string.IsNullOrWhiteSpace(displayname))
                {
                    existsDados.DisplayName = displayname;
                }

                if (!string.IsNullOrWhiteSpace(cpf))
                {
                    existsDados.CPF = cpf;
                }

                if (!string.IsNullOrWhiteSpace(phonenumber))
                {
                    existsDados.PhoneNumber = phonenumber;
                }
            }

            await _db.SaveChangesAsync();

            return new
            {
                conta = user.Name,
                nomeExibicao = displayname,
                cpfConta = cpf,
                phoneNumber = phonenumber
            };
        }

        public async Task<List<AccountResponseDto>> ConsultarDados(string email)
        {
            var user = await _authService.FindByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("A conta informada não foi localizada.");
            }

            var existsDados = await FindDadosById(user.Id);
            if (existsDados == null)
            {
                throw new InvalidOperationException("A conta informada não foi localizada.");
            }

            var dados = await _db.Dados.Where(ac => ac.UsuarioId == user.Id).Select(ac => new AccountResponseDto
            {
                Id = ac.Id,
                DisplayName = ac.DisplayName,
                CPF = ac.CPF,
                PhoneNumber = ac.PhoneNumber,
                CreatedAt = ac.CreatedAt
            }).ToListAsync();

            return dados;
        }
    }
}