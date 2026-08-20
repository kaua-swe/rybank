using rybank.Dto.Account;
using rybank.Models.Account;

namespace rybank.Interfaces.Account
{
    public interface IAccountService
    {
        Task<AccountModel?> FindDadosById(Guid usuarioId);
        Task<List<AccountResponseDto>> ConsultarDados(string email);
        Task<object> AtualizarDados(string email, string? displayname, string? cpf, string? phonenumber);
    }
}