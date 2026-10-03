using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    public class Otp
    {
        [Key]
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; }

        public int AttemptCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
