namespace rybank.estudo.Dto
{
    public class TransferDto
    {
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public decimal Valor { get; set; } = 0.00m;
    }
}