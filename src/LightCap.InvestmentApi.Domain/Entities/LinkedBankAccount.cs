using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    public class LinkedBankAccount
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        // The permanent Account ID returned by Mono after exchanging the temporary code.
        // This is what you pass to DirectPay later to charge this account.
        public string MonoAccountId { get; set; } = string.Empty;

        public DateTime LinkedAt { get; set; }
        public bool IsActive { get; set; }

        public User? User { get; set; }
    }
}
