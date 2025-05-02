using System.Text.Json.Serialization;

namespace Furia_FanHub.MVVM.Models
{
    public class UserAddress
    {
        public string CEP { get; set; }

        [JsonPropertyName("logradouro")]
        public string Rua { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}
