using src.DTOs.Ticket;

namespace src.Interfaces.Ticket
{
    public interface ITicketService
    {
        Task<object> CreateTicket(string empresa, string cnpj, int vencimento, decimal valor);
        Task<object> PayTicket(string email, string codigo, string conta);
        Task<object> CancellTicket(string codigo);
        Task<List<ResponseTicketDto>> ListTicket(string empresa);
    }
}