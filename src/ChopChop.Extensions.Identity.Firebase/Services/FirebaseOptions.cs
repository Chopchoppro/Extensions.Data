

using System.ComponentModel;

using Newtonsoft.Json;


public class FirebaseOptions
{
    [JsonIgnore]
    public string Url { get { return "https://identitytoolkit.googleapis.com/v1/"; } }

    [JsonProperty("project_id")]
    public string ApiKey { set; get; }

    [JsonIgnore]
    public string Audience { set; get; }

    [JsonIgnore]
    public string Validator { get { return $"https://securetoken.google.com/{Audience}"; } }

    [JsonProperty("type")]
    public string Type { set; get; }

    private string private_key_id;
    [JsonProperty("private_key_id")]
    public string PrivateKeyId
    {
        get
        {
            if (private_key_id == null)
                throw new InvalidEnumArgumentException("Private Key Is null");
            if (!private_key_id.StartsWith("-----BEGIN PRIVATE KEY-----"))
                private_key_id = "-----BEGIN PRIVATE KEY-----\n" + private_key_id;
            if (!private_key_id.EndsWith("-----END PRIVATE KEY-----"))
                private_key_id = private_key_id+"\n-----END PRIVATE KEY-----\n" ;

            return private_key_id.Replace("\\n", "\n");
        }
        set
        {
            private_key_id = value;
        }
    }

    [JsonProperty("private_key")]
    public string PrivateKey { get; set; }

    [JsonProperty("client_email")]
    public string ClientEmail { get; set; }

    [JsonProperty("client_id")]
    public string ClientId { get; set; }

    [JsonProperty("auth_uri")]
    public string AuthUri { set; get; }

    [JsonProperty("token_uri")]
    public string TokenUri { get; set; }

    [JsonProperty("auth_provider_x509_cert_url")]
    public string AuthProvider { get; set; }

    [JsonProperty("client_x509_cert_url")]
    public string Client { get; set; }

    [JsonProperty("universe_domain")]
    public string UniverseDomain { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this).Replace("\\n", "\n");
    }
}
