using LightCap.InvestmentApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LightCap.InvestmentApi.Domain.Entities
{
    public class WalletTransaction
    {

        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }   // denormalized for easy querying without a join

        public WalletTransactionType Type { get; set; }
        public WalletTransactionStatus Status { get; set; }

        // The amount actually debited (after the ₦200 floor is applied) -
        // NOT necessarily the raw calculated percentage. See Description
        // for the raw calculated amount, for transparency/audit purposes.
        public decimal Amount { get; set; }

        // Snapshot for audit trail - what AvailableBalance was AFTER this transaction.
        public decimal AvailableBalanceAfter { get; set; }

        // Ties this transaction back to its source, and is critical for idempotency:
        // the Mono transaction ID that triggered this debit. This is what stops
        // the same spend webhook from being processed twice.
        public string SourceReference { get; set; } = string.Empty;

        // The reference sent TO Mono for this specific debit call - needed to
        // match up the later debit-confirmation webhook to this exact row.
        public string? MonoDebitReference { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public Wallet? Wallet { get; set; }

    }
}



