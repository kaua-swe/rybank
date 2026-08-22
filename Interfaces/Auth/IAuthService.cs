namespace src.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<object> CreateUser(string email, string senha);
        Task<object> Authentication(string email, string senha);
    }
}