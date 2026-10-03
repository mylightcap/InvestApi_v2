using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Auth.Registration
{
    public class UserRegistrationDto
    {
        public required string FirstName { get; set; } 

        public required string LastName { get; set; } 

        public  string? MiddleName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; } 

        public  string Password { get; set; } 

        public  string ConfirmPassword { get; set; } 

        public bool AcceptTermsAndConditions { get; set; }

        public bool AcceptPrivacyPolicy { get; set; }

        public string DeviceId { get; set; } = default!;

        public string DeviceName { get; set; } = default!;

        public string DeviceType { get; set; } = default!;

 
        public string IpAddress { get; set; } = default!;

        public string Country { get; set; } = default!;

        public string? State { get; set; }

        public string? City { get; set; }

       
    
}
}
