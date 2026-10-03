using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsCreated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public Guid DeletedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public string ModifiedByName { get; set; } = string.Empty;
        public Guid? LastModifiedBY { get; set; }
        public DateTime LastModified { get; set; }
    }
}
