using System.Text.Json.Serialization;

namespace Furia_FanHub.MVVM.Models
{
    public class ChatMessage
    {
        [JsonPropertyName("Choice")]
        public string Text { get; set; }
        public bool IsUser { get; set; }
        public bool IsSystemMessage { get; set; } = false;
    }
}
