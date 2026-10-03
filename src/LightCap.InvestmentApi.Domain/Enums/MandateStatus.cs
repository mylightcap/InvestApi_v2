using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Enums
{
    public enum MandateStatus
    {
        Pending,        // Created with Mono, but not yet confirmed debitable
        ReadyToDebit,   // Mono confirmed - this is when debits are actually allowed
        Failed,
        Cancelled
    }
}
