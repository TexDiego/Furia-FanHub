using System.Text.Json.Serialization;

namespace Furia_FanHub.MVVM.Models
{
    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }
}
