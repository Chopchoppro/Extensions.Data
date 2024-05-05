using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChopChop.Extensions.Data.Model.Entity;

public class BaseStatusEntity : BaseLog
{
    public Guid Id { get; set; } 
    public Status Status { get; set; }
    public int Version { get; set; }
}
