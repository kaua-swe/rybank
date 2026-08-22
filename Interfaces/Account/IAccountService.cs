using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Model.Account;

namespace src.Interfaces.Account
{
    public interface IAccountService
    {
        Task<AccountModel?> FindAccountById(Guid usuarioId);
        Task<object> CreateAccount(string email, string nomeCompleto, string cpf, string telefone);
        Task<object> UpdateAccount(string email, string nomeCompleto, string cpf, string telefone);
    }
}