namespace src.DTOs.Ticket
{
    public class ResponseTicketDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Empresa { get; set; } = string.Empty;

        public string Codigo { get; set; } = string.Empty;

        public string CNPJ { get; set; } = string.Empty;

        public DateTime Vencimento { get; set; }

        public decimal Valor { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}