using rybank.Dto.Pay;
using rybank.estudo.Models;

namespace rybank.estudo.Interfaces
{
    public interface IBankService
    {
        Task<SaldoModel?> FindSaldoById(Guid usuarioId);
        Task<TransferenciaModel?> FindTransferById(Guid usuarioId);
        Task<BoletoModel> VerifiyStatusBoleto(BoletoModel boleto);
        Task<object> Deposito(string email, decimal valor);
        Task<object> Sacar(string email, decimal valor);
        Task<object> Transferir(string origem, string tipo, string? destino, string? chave, decimal valor);
        Task<object> GerarBoleto(string empresa, string devedor, decimal valor, int vencimento);
        Task<object> PagarBoleto(string conta, string codigo);        
        Task<object> CancelarBoleto(string codigo);
        Task<List<TransferResponseDto>> ListarTransferencias(string email);
        
    }
}