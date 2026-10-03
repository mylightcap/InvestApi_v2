using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Enums
{
    public enum WalletTransactionType
    {
        // pendRoundUp -> A small credit added to PendingRoundUpBalance after a detected purchase.
        // e.g. user spends ₦1,000, auto-invest is 5% -> +₦50 here.
        // This does NOT move any real money - pure bookkeeping.

        // ThresholdDebit -> Fired when PendingRoundUpBalance crosses the ₦1,000 threshold and a
        // real DirectPay call is made to pull that amount from the user's bank.
        // Status starts Pending, then becomes Completed/Failed once Mono confirms.
        // On success: PendingRoundUpBalance decreases, AvailableBalance increases.

        // InvestmentExecuted -> Fired when AvailableBalance is sent to the investment partner.
        // On success: AvailableBalance decreases, TotalInvested increases.

        // Reversal-> A correction/refund of a previous entry, if ever needed.

        SpendRoundUp,
        ThresholdDebit,
        InvestmentExecuted,
        Reversal
    }
}
