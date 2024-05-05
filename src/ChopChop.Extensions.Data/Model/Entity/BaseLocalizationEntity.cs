using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChopChop.Extensions.Data.Model.Entity;

public abstract class BaseLocalizationResponse : BaseStatusEntity
{
    public Guid LocaliztionId { get; set; }

    public string Culture { get; set; }
}
