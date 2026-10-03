using LightCap.InvestmentApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    // Tracks the Direct Debit permission itself, separate from the User table -
    // a user could only be allowed one active mandate, but keeping this as its
    // own table makes the history/audit trail clean if a mandate is ever
    // cancelled and re-created.
    public class DirectDebitMandate
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string MonoMandateId { get; set; } = string.Empty;
        public MandateStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ReadyAt { get; set; }

        public User? User { get; set; }
    }
}
