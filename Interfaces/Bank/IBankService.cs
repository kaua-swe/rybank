using src.Enums.Bank;
using src.Model.Auth;
using src.Model.Bank;

namespace src.Interfaces.Bank
{
    public interface IBankService
    {
        TypeWalletEnum VerifyTypeWallet(string wallet);
        Task<WalletModel?> FindWalletById(Guid usuarioId);
        Task<UserModel?> FindUserByEmail(string email);
        Task<object> CreateWallet(string email, string typeWallet);
        Task<object> Deposit(string email, decimal valor, string conta);
        Task<object> Transfer(string email, string tipo, string chave, string conta, decimal valor);
        Task<object> Sacar(string email, string tipoConta, decimal valor);
    }
}