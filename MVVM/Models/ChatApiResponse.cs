using System.Text.Json.Serialization;

namespace Furia_FanHub.MVVM.Models
{
    public class ChatApiResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }
}
