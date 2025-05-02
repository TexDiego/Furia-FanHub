using System.Text.Json.Serialization;

namespace Furia_FanHub.MVVM.Models
{
    public class GoogleUserInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
