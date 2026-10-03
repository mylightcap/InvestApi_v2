using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Auth.Login.Commands
{
    public class UserLoginResponse
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; } 

        public string Email { get; set; } 

        public string AccessToken { get; set; } 

        public string RefreshToken { get; set; } 

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
