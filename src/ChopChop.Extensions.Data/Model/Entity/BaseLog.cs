using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChopChop.Extensions.Data.Model
{
    public abstract class BaseLog
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
