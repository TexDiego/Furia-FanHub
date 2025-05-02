using System.Text.Json;

namespace Furia_FanHub.MVVM.Models.Services
{
    public class CEPService
    {
        public async Task<UserAddress> BuscarEnderecoPorCep(string cep)
        {
            using var httpClient = new HttpClient();
            string url = $"https://viacep.com.br/ws/{cep}/json/";

            var response = await httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(response))
            {
                var endereco = JsonSerializer.Deserialize<UserAddress>(response);
                return endereco;
            }

            return null;
        }
    }
}
