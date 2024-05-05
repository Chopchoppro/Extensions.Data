using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Newtonsoft.Json;

namespace ChopChop.Extensions.Data;


public static class Extensions
{
    public static string ToJsonString<T>(this T @object)
    {
        if (@object == null)
        {
            throw new ArgumentNullException("object");
        }

        return JsonConvert.SerializeObject(@object, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }

    public static T FromJsonString<T>(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentNullException("json");
        }

        return JsonConvert.DeserializeObject<T>(json);
    }
}
