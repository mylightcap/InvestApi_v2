using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Enums
{
    public enum WalletTransactionStatus
    {
        Pending,    // Recorded, but the money movement hasn't been confirmed yet
        Completed,  // Confirmed successful
        Failed      // Attempted but failed
    }
}
