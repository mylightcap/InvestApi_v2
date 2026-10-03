using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    public class Wallet
    {


        // One Wallet per user.
        //
        // IMPORTANT - this entity holds TWO different balances, and they must
        // never be confused with each other:
        //
        //   PendingRoundUpBalance -> NOT real money. Just a running total of
        //   round-up percentages accumulating toward the ₦1,000 threshold.
        //   Nothing has been pulled from the user's real bank account yet.
        //
        //   AvailableBalance -> REAL money. This only increases once a
        //   DirectPay debit has been confirmed successful - i.e. money has
        //   actually left the user's bank account and landed with LightCap.
        //
        // Never update either balance directly from random places in the code -
        // always go through a WalletTransaction first (the ledger), then update
        // the relevant balance here. The ledger is the source of truth.

        // PendingRoundUpBalance -> NOT real money. Accumulates from small round-up percentages after
        // every detected purchase. Resets toward zero once it crosses the
        // threshold(eg: >= 1000) and a real DirectPay debit is confirmed.

        // AvailableBalance -> REAL money. Only increases when a DirectPay debit is confirmed
        // successful. This is what's actually sitting with LightCap, ready
        // to be sent to the investment partner.

        public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public decimal PendingRoundUpBalance { get; set; }
            public decimal AvailableBalance { get; set; }        
            public decimal TotalInvested { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public User? User { get; set; }
        
    }
}

