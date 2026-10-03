using LightCap.InvestmentApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? MiddleName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public required string PasswordHash { get; set; }

        public bool IsEmailVerified { get; set; }

        public bool AcceptTermsAndConditions { get; set; }

        public bool AcceptPrivacyPolicy { get; set; }
        public decimal AutoInvestPercentage { get; set; }
        public InvestmentMode InvestmentMode { get; set; } = InvestmentMode.UserDirected; // sensible default

        // NEW: required by Mono to create a Direct Debit Customer profile and,
        // later, a mandate. Nullable because it won't be collected at registration -
        // it's captured as a separate step, right before the user sets up
        // automatic debits (mandate setup), not during signup.
        public string? Bvn { get; set; }

        // NEW: tracks whether this user has completed Direct Debit mandate setup.
        // Populated once the mandate is created (see MonoCustomerId/MandateId below).
        // Kept here for a quick "can this user be auto-debited?" check without
        // a join, though the authoritative record lives in a separate
        // DirectDebitMandate table (built alongside the mandate-setup feature).
        public string? MonoCustomerId { get; set; }

        public string DeviceId { get; set; } = default!;

        public string DeviceName { get; set; } = default!;

        public string DeviceType { get; set; } = default!;

        public string IpAddress { get; set; } = default!;

        public string Country { get; set; } = default!;

        public string? State { get; set; }

        public string? City { get; set; }
    }
}
