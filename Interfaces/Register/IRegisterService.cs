using src.DTOs.Ticket;

namespace src.Interfaces.Register
{
    public interface IRegisterService
    {
        Task<object> CreateUser(string email, string senha, string nomeCompleto, string cpf, string telefone, string tipoConta);
        Task<object> UpdateUser(string email, string nomeCompleto, string cpf, string telefone);
        Task<object> Authentication(string email, string senha);
        Task<object> CreateWallet(string email, string tipoConta);
        Task<object> Deposit(string email, decimal valor, string tipoConta);
        Task<object> RegisterKeyPix(string email, string tipoChave, string chave, string conta);
        Task<object> Transfer(string email, string tipo, string chave, string conta, decimal valor);
        Task<object> Sacar(string email, string tipoConta, decimal valor);
        Task<object> DeleteKey(string email, string chave);
        Task<object> CreateTicket(string empresa, string cnpj, int vencimento, decimal valor);
        Task<object> PayTicket(string email, string codigo, string conta);
        Task<object> CancellTicket(string codigo);
        Task<List<ResponseTicketDto>> ListTicket(string empresa);
    }
}