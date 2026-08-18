namespace rybank.Dto.Pix
{
    public class PixResponseDto
    {
        public Guid Id { get; set; }

        public string Chave { get; set; } = string.Empty;

        public string TipoChave { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}