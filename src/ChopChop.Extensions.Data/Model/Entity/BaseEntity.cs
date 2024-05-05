using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChopChop.Extensions.Data.Model.Entity;


public abstract class BaseEntity : BaseStatusEntity
{
    public Guid AppId { get; set; }
    public string UserName { get; set; } 
}

public enum Status
{
    Deleted = 0,
    Active = 1
}
