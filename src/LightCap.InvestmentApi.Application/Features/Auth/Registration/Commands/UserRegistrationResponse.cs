using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Auth.Registration.Commands
{
    public class UserRegistrationResponse
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public bool IsEmailVerified { get; set; }

        public bool IsPhoneVerified { get; set; }

        public bool RequiresOtpVerification { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Message { get; set; } = default!;
    }
}
