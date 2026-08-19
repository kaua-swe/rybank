using rybank.Enums.Pay;

namespace rybank.estudo.Dto
{
    public class TransferDto
    {
        public string Origem { get; set; } = string.Empty;
        public TipoPagamento Tipo { get; set; }
        public string Destino { get; set; } = string.Empty;
        public string? Chave { get; set; }
        public decimal Valor { get; set; } = 0.00m;
    }
}