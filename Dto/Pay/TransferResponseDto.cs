using System.ComponentModel.DataAnnotations.Schema;

namespace rybank.Dto.Pay
{
    public class TransferResponseDto
    {
        public Guid Id { get; set; }

        public Guid OrigemUserId { get; set; }

        public Guid DestinoUserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}