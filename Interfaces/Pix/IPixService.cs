using src.Enums.Bank;
using src.Enums.Pix;
using src.Model.Account;
using src.Model.Auth;
using src.Model.Bank;
using src.Model.Pix;

namespace src.Interfaces.Pix
{
    public interface IPixService
    {
        TypePixEnum VerifyTypePix(string tipoPix);

        TypeWalletEnum VerifyTypeWallet(string tipoConta);

        Task<PixModel?> FindPixById(Guid usuarioId);

        Task<UserModel?> FindIdByEmail(string email);

        Task<AccountModel?> FindAccountById(Guid usuarioId);
        

        Task<WalletModel?> FindWalletById(Guid usuarioId);


        Task<object> RegisterKeyPix(string email, string tipoChave, string chave, string conta);
        Task<object> DeleteKey(string email, string chave);
    }
}