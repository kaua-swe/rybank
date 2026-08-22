using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Interfaces.Auth;
using src.Model.Auth;

namespace src.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserModel?> FindByEmail(string email)
        {
            var user = await _db.User.FirstOrDefaultAsync(e => e.Email == email);
            return user;
        }

        public async Task<object> CreateUser(string email, string senha)
        {
            var userAlreadyExists = await FindByEmail(email);
            if (userAlreadyExists != null)
            {
                throw new InvalidOperationException("Conta informada já cadastrada.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(senha);
            var utcNow = DateTime.UtcNow;
            var userEmail = email.Trim();
            var newUser = new UserModel
            {
                Email = userEmail,
                Senha = passwordHash,
                CreatedAt =  utcNow,
                UpdatedAt = utcNow
            };

            _db.User.Add(newUser);
            await _db.SaveChangesAsync();

            return new
            {
                newUser.Id,
                newUser.Email,
                newUser.CreatedAt
            };
        }

        public async Task<object> Authentication(string email, string senha)
        {
            var emailUser = email.Trim();
            var user = await FindByEmail(emailUser);
            if (user == null)
            {
                throw new InvalidOperationException("Credenciais inválidas.");
            }
            var matchPassword = BCrypt.Net.BCrypt.Verify(senha, user.Senha);
            if (!matchPassword)
            {
                throw new InvalidOperationException("Credenciais inválidas.");
            }

            return new
            {
                user.Email,
                user.UpdatedAt
            };
        }
    }
}