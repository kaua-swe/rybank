using rybank.estudo.Models;

namespace rybank.estudo.Interfaces
{
    public interface IAuthService
    {
        Task<UserModel?> FindByEmail(string email);
        Task<object> CreateUser(string nome, string email, string password);
        Task<object> Authentication(string email, string password);
    }
}