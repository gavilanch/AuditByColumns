using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditEFCore.Models
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public string CreatedByUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
