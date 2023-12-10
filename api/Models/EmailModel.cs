using Newtonsoft.Json;

namespace Company.Function.Models
{
    public record EmailModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
