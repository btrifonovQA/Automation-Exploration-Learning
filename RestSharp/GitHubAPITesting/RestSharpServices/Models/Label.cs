using System.Text.Json.Serialization;

namespace RestSharpServices.Models
{
    public class Label
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
