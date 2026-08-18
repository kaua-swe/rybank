using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rybank.Dto.Account
{
    public class AccountResponseDto
    {
        public Guid Id { get; set;}

        public string? DisplayName { get; set; }

        public string? CPF { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}