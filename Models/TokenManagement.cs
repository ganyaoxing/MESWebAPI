namespace MESWebAPI.Models;
using Newtonsoft.Json;
#nullable disable
public class TokenManagement
{
    [JsonProperty("secret")]
    public string Secret { get; set; }
    [JsonProperty("issuer")]
    public string Issuer { get; set; }
    [JsonProperty("audience")]
    public string Audience { get; set; }
    [JsonProperty("accessexpiration")]
    public int AccessExpiration { get; set; }
    [JsonProperty("refreshexpiration")]
    public int RefreshExpiration { get; set; }
}
