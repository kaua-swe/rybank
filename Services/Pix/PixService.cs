using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    }
}