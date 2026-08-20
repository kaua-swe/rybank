using Microsoft.EntityFrameworkCore;
using rybank.Dto.Pix;
using rybank.Enums;
using rybank.estudo.Data;
using rybank.estudo.Interfaces;
using rybank.Interfaces;
using rybank.Interfaces.Account;
using rybank.Models;

namespace rybank.Services
{
    public class PixService : IPixService
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public PixService(AppDbContext db, IAuthService authService, IAccountService accountService)
        {
            _db = db;
            _authService = authService;
            _accountService = accountService;
        }

        public async Task<PixModel?> FindPixById(Guid usuarioId)
        {
            var existsPix = await _db.Pix.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

            return existsPix;
        }

        public ChavePixEnum VerificarTipoChave(string tipoChave)
        {

            if (!Enum.TryParse<ChavePixEnum>(tipoChave, true, out var tipo))
            {
                throw new InvalidOperationException("Não foi encontrado este tipo de chave.");
            }

            return tipo;
        }

        public async Task<PixModel?> FindChave(string chave)
        {
            var existsChave = await _db.Pix.FirstOrDefaultAsync(p => p.Chave == chave);
            
            return existsChave;
        }

        public async Task<object> CreatePix(string email, string tipoChave)
        {
            var tipo = VerificarTipoChave(tipoChave);

            var user = await _authService.FindByEmail(email);

            if (user == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para cadastro da chave.");
            }

            var existsDados = await _accountService.FindDadosById(user.Id);
            if (existsDados == null)
            {
                throw new InvalidOperationException("Esta conta não possui informações para cadastrar chaves Pix.");
            }

            if (existsDados.CPF == null || existsDados.Email == null)
            {
                throw new InvalidOperationException("Esta conta não possui informações para cadastrar chaves Pix.");
            }

            var tipoJaCadastrado = await _db.Pix.AnyAsync(pix => pix.UsuarioId == user.Id && pix.TipoChave == tipo);

            if (tipoJaCadastrado)
            {
                throw new InvalidOperationException($"A conta já possui o tipo {tipo} cadastrado.");
            }

            string chave;
            if (tipo == ChavePixEnum.CPF)
            {
                if(string.IsNullOrWhiteSpace(existsDados.CPF))
                {
                    throw new InvalidOperationException("A conta não possui CPF cadastrado.");
                }
                chave = existsDados.CPF;
            } else if (tipo == ChavePixEnum.Email)
            {
                if(string.IsNullOrWhiteSpace(existsDados.Email))
                {
                    throw new InvalidOperationException("A conta não possui email cadastrado.");
                }
                chave = existsDados.Email;
            } else
            {
                throw new InvalidOperationException("Valor de chave inválido.");
            }

            var newChave = new PixModel
            {
                UsuarioId = user.Id,
                Chave = chave,
                TipoChave = tipo,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            _db.Pix.Add(newChave);

            await _db.SaveChangesAsync();

            return new
            {
                Id = newChave.Id,
                UsuarioId = newChave.UsuarioId,
                Chave = newChave.Chave,
                TipoChave = newChave.TipoChave.ToString(),
                Status = newChave.Status.ToString(),
                CreatedAt = newChave.CreatedAt
            };
            
        }

        public async Task<List<PixResponseDto>> Consultar(string email)
        {
            var user = await _authService.FindByEmail(email);

            if (user == null)
            {
                throw new InvalidOperationException("Não encontrada a conta.");
            }

            var existsPix = await FindPixById(user.Id);

            if (existsPix == null)
            {
                throw new InvalidOperationException("Não encontrada a conta.");
            }

            var chaves = await _db.Pix.Where(pix => pix.UsuarioId == user.Id).Select(pix => new PixResponseDto
            {
                Id = pix.Id,
                Chave = pix.Chave,
                TipoChave = pix.TipoChave.ToString(),
                Status = pix.Status.ToString(),
                CreatedAt = pix.CreatedAt
            }).ToListAsync();

            return chaves;
        }

        public async Task<object> Deletar(string email, string tipoChave)
        {
            var user = await _authService.FindByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Não encontrada a conta.");
            }

            var existsPix = await FindPixById(user.Id);

            if (existsPix == null)
            {
                throw new InvalidOperationException("Não encontrada o pix.");
            }

            var tipo = VerificarTipoChave(tipoChave);

            var tipoJaCadastrado = await _db.Pix.AnyAsync(pix => pix.UsuarioId == user.Id && pix.TipoChave == tipo);

            if (!tipoJaCadastrado)
            {
                throw new InvalidOperationException("Não encontrada o tipo de chave cadastrada.");
            }

            _db.Pix.Remove(existsPix);
            await _db.SaveChangesAsync();

            return new
            {
                message = "Removido a chave da conta"
            };
        }
    }
}