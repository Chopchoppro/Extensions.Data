using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChopChop.Extensions.Data.Model.Entity;

namespace ChopChop.Extensions.Data.Model.Response;

public abstract class BaseLocalizationResponse<T> where T : BaseLocalization, new() 
{
    public IEnumerable<T> Localization { get; set; }
}
