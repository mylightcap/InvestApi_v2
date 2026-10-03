using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Auth.Login.Commands
{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string IpAddress { get; set; } 
        public string DeviceId { get; set; } 
        public string DeviceName { get; set; } 
    }
}
