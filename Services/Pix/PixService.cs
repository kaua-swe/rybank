using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Enums.Bank;
using src.Enums.Pix;
using src.Interfaces.Pix;
using src.Model.Account;
using src.Model.Auth;
using src.Model.Bank;
using src.Model.Pix;

namespace src.Services.Pix
{
    public class PixService : IPixService
    {
        private readonly AppDbContext _db;

        public PixService(AppDbContext db)
        {
            _db = db;
        }

        public TypePixEnum VerifyTypePix(string tipoPix)
        {
            if (!Enum.TryParse<TypePixEnum> ( tipoPix, true, out var tipo))
            {
                throw new InvalidOperationException($"Não identificado o pix {tipoPix}");
            }

            return tipo;
        }

        public TypeWalletEnum VerifyTypeWallet(string tipoConta)
        {
            if (!Enum.TryParse<TypeWalletEnum> ( tipoConta, true, out var tipo))
            {
                throw new InvalidOperationException($"Não identificado conta {tipoConta}");
            }

            return tipo;
        }

        public async Task<PixModel?> FindPixById(Guid usuarioId)
        {
            var user = await _db.Pix.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
            return user;
        }

        public async Task<UserModel?> FindIdByEmail(string email)
        {
            var userEmail = await _db.User.FirstOrDefaultAsync(u => u.Email == email);
            return userEmail;
        }

        public async Task<AccountModel?> FindAccountById(Guid usuarioId)
        {
            var account = await _db.Account.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
            return account;
        }

        public async Task<WalletModel?> FindWalletById(Guid usuarioId)
        {
            var wallet = await _db.Wallet.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
            return wallet;
        }

        public async Task<WalletModel?> FindWalletByIdFilter(Guid usuarioId, string conta)
        {
            var wallet = await _db.Wallet.FirstOrDefaultAsync(w => w.UsuarioId == usuarioId && (w.Conta == conta));
            return wallet;
        }


        public async Task<object> RegisterKeyPix(string email, string tipoChave, string chave, string conta)
        {
            var utcNow = DateTime.UtcNow;
            var emailUser = email.Trim();
            var userEmail = await FindIdByEmail(emailUser);
            if (userEmail == null)
            {
                throw new InvalidOperationException("Não identificado a conta para continuar a operação.");
            }

            var existsAccount = await FindAccountById(userEmail.Id);
            if (existsAccount == null)
            {
                throw new InvalidOperationException("Não identificado a conta para continuar a operação.");
            }

            var tipoConta = VerifyTypeWallet(conta);
            string wallet = tipoConta.ToString();

            var existsWallet = await FindWalletByIdFilter(userEmail.Id, wallet);

            if (existsWallet == null)
            {
                throw new InvalidOperationException("Não identificado a conta para continuar a operação.");
            }

            var tipoPix = VerifyTypePix(tipoChave);
            string tipo = tipoPix.ToString(); 
            
            if (tipo == TypePixEnum.CPF.ToString())
            {
                if (chave != existsAccount.CPF)
                {
                    throw new InvalidOperationException($"O documento {chave} não bate com o registro da conta.");
                }

                var existsChave = await _db.Pix.AnyAsync(p => p.UsuarioId == userEmail.Id && (p.Chave == chave));
                if (existsChave)
                {
                    throw new InvalidOperationException($"A chave {chave} não pode ser utilizada.");
                }

                var newChave = new PixModel
                {
                    UsuarioId = userEmail.Id,
                    Conta = wallet,
                    Chave = existsAccount.CPF,
                    Status = StatusPixEnum.ATIVO.ToString(),
                    CreatedAt = utcNow,
                    UpdatedAt = utcNow
                };

                _db.Pix.Add(newChave);
                await _db.SaveChangesAsync();

                return new
                {
                    newChave.Conta,
                    newChave.Chave,
                    newChave.CreatedAt
                };
            }
            if (tipo == TypePixEnum.EMAIL.ToString())
            {
                
                var existsChave = await _db.Pix.FirstOrDefaultAsync(c => c.Chave == chave);
                if (existsChave != null)
                {
                    if (existsChave.Status == StatusPixEnum.ATIVO.ToString())
                    {
                        throw new InvalidOperationException($"A chave {chave} não pode ser utilizada.");
                    }
                    
                }
                var alreadyRegister = await _db.Pix.AnyAsync(p => p.UsuarioId == userEmail.Id && (p.Chave == chave));
                if (alreadyRegister)
                {
                    throw new InvalidOperationException($"A conta já possui o registro {chave}.");
                }
                var newChave = new PixModel
                {
                    UsuarioId = userEmail.Id,
                    Conta = wallet,
                    Chave = chave,
                    Status = StatusPixEnum.PENDENTE.ToString(),
                    CreatedAt = utcNow,
                    UpdatedAt = utcNow
                };

                _db.Pix.Add(newChave);
                await _db.SaveChangesAsync();

                return new
                {
                    newChave.Conta,
                    newChave.Chave,
                    newChave.CreatedAt
                };
            }
            throw new InvalidOperationException($"Não identificado o cadastro {tipo}");
        }

        public async Task<object> DeleteKey(string email, string chave)
        {
            var emailUser = email.Trim();
            var userAlreadyExists = await FindIdByEmail(emailUser);
            if (userAlreadyExists == null)
            {
                throw new InvalidOperationException("Não identificado a conta para continuar a operação.");   
            }

            var existsChave = await _db.Pix.FirstOrDefaultAsync(u => u.UsuarioId == userAlreadyExists.Id && (u.Chave == chave));
            if (existsChave == null)
            {
                throw new InvalidOperationException($"Não identificado a chave {chave} cadastrada.");
            }

            _db.Pix.Remove(existsChave);
            await _db.SaveChangesAsync();

            return new
            {
                Conta = userAlreadyExists.Email,
                ChaveRemovida = chave
            };
        }
    }
}