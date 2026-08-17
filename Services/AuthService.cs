using Microsoft.EntityFrameworkCore;
using rybank.estudo.Data;
using rybank.estudo.Interfaces;
using rybank.estudo.Models;

namespace rybank.estudo.Services
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
            var userAlreadyExists = await _db.Users.FirstOrDefaultAsync(user => user.Email == email);
            return userAlreadyExists;
        }

        public async Task<object> CreateUser(string nome, string email, string password)
        {

            var userAlreadyExists = await FindByEmail(email);

            if (userAlreadyExists != null)
            {
                throw new InvalidOperationException("E-mail já cadastrado.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new UserModel
            {
                Name = nome,
                Email = email,
                Senha = passwordHash
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new
            {
                user.Id,
                user.Name,
                user.Email
            };
        }

        public async Task<object> Authentication(string email, string password)
        {
            var userAlreadyExists = await FindByEmail(email);

            if (userAlreadyExists == null)
            {
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
            }

            var passwordMatch = BCrypt.Net.BCrypt.Verify(password, userAlreadyExists.Senha);

            if (!passwordMatch)
            {
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
            }

            return new
            {
                userAlreadyExists.Name,
                userAlreadyExists.Email
            };
        }
    }
}