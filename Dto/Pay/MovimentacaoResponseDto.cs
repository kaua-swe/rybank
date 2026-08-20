using rybank.estudo.Enums;

namespace rybank.Dto.Pay
{
    public class MovimentacaoResponseDto
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public decimal Valor { get; set; }

        public string Tipo { get; set; } = string.Empty;
    }
}