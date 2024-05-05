using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChopChop.Extensions.Data.Model;


public abstract class BaseEntity : BaseLog
{
    public Guid Id { get; set; }
    public Guid AppId { get; set; }
    public string UserName { get; set; }
    public Status Status { get; set; }
    public int Version { get; set; } 
}

public enum Status
{
    Deleted = 0,
    Active = 1
}
