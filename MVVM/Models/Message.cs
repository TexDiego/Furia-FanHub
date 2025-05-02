using System.Text.Json.Serialization;

namespace Furia_FanHub.MVVM.Models
{
    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
