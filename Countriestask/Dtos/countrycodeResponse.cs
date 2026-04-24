using System.Text.Json.Serialization;

namespace Countriestask.Dtos
{
    public class countrycodeResponse
    {

        [JsonPropertyName("country_code2")]
        public string? Country_Code2{ get; set; }
    }
}
