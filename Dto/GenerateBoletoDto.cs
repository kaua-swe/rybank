namespace rybank.estudo.Dto
{
    public class GenerateBoletoDto
    {
        public string Empresa { get; set; } = string.Empty;
        public string Devedor { get; set; } = string.Empty;
        public decimal ValorBoleto { get; set; } = 0.00m;
        public int Vencimento { get; set;}
    }
}