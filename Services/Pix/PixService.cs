using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rybank.Dto.Pix;
using rybank.Enums;
using rybank.estudo.Data;
using rybank.estudo.Interfaces;
using rybank.Interfaces;
using rybank.Models;

namespace rybank.Services
{
    public class PixService : IPixService
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;

        public PixService(AppDbContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
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

        public async Task<object> CreatePix(string email, string tipoChave, string valorChave)
        {
            var tipo = VerificarTipoChave(tipoChave);

            var user = await _authService.FindByEmail(email);

            if (user == null)
            {
                throw new InvalidOperationException("Não encontrado a conta para cadastro da chave.");
            }

            var tipoJaCadastrado = await _db.Pix.AnyAsync(pix => pix.UsuarioId == user.Id && pix.TipoChave == tipo);

            if (tipoJaCadastrado)
            {
                throw new InvalidOperationException($"A conta já possui o tipo {tipo} cadastrado.");
            }

            var newChave = new PixModel
            {
                UsuarioId = user.Id,
                Chave = valorChave,
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
                throw new InvalidOperationException("Não encontrada a conta.");
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