using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ChopChop.Extensions.Identity.Firebase.Models;


public  class VerifyCustomTokenModel
{
    [JsonProperty("idToken")]
    public string IdToken { get; set; }
     

    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }

    [JsonProperty("expiresIn")]
    public string ExpiresIn { get; set; }
 
}
