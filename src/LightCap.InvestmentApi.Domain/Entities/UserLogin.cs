using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    public class UserLogin
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Email { get; set; } = default!;

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public bool IsActive { get; set; }

        public string IpAddress { get; set; } = default!;

        public string DeviceId { get; set; } = default!;

        public string DeviceName { get; set; } = default!;

        // =========================
        // SECURITY FLAGS
        // =========================

        public bool IsSuspicious { get; set; }

        public string? FailureReason { get; set; }


        // =========================
        // TOKEN TRACKING (OPTIONAL BUT IMPORTANT)
        // =========================

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
