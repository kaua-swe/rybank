using rybank.estudo.Models;

namespace rybank.estudo.Interfaces
{
    public interface IBankService
    {
        Task<SaldoModel?> FindSaldoById(Guid usuarioId);
        Task<object> Deposito(string email, decimal valor);
        Task<object> Sacar(string email, decimal valor);
        Task<object> Transferir(string origem, string destino, decimal valor);
        Task<object> GerarBoleto(string empresa, string devedor, decimal valor, int vencimento);
        Task<object> PagarBoleto(string conta, string codigo);
        Task<BoletoModel> VerifiyStatusBoleto(BoletoModel boleto);
        Task<object> CancelarBoleto(string codio);
    }
}