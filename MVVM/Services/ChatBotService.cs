using Furia_FanHub.MVVM.Models;
using System.Text;
using System.Text.Json;

namespace Furia_FanHub.MVVM.Services
{
    public class ChatBotService
    {
        private readonly string api_key;
        private readonly Secrets secrets;

        public ChatBotService()
        {
            secrets = new Secrets();
            api_key = secrets.OPENAI_API_KEY;
        }

        public async Task<string?> RequestConnection(string prompt, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(prompt)) return null;

                Console.WriteLine($"API: {api_key}");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + api_key);

                    var content = new
                    {
                        model = "gpt-3.5-turbo",  // Alterando para o modelo gpt-3.5-turbo
                        messages = new[]
                        {
                    new { role = "system", content = role },
                    new { role = "user", content = prompt }
                },
                        temperature = 0.7
                    };

                    var jsonContent = JsonSerializer.Serialize(content);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Resposta da API: " + responseString);

                        var data = JsonSerializer.Deserialize<ChatApiResponse>(responseString);

                        if (data != null && data.Choices != null && data.Choices.Count > 0)
                        {
                            var botMessage = data.Choices[0].Message.Content.Trim();
                            Console.WriteLine("Resposta do bot: " + botMessage);
                            return botMessage;
                        }
                        else
                        {
                            Console.WriteLine("Erro: Resposta do bot não encontrada ou sem escolhas.");
                            return "Erro: Nenhuma resposta válida do bot.";
                        }
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Erro HTTP: {response.StatusCode}, Resposta: {errorResponse}");
                        return "Erro: Nenhuma resposta válida do bot.";
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Erro na requisição: " + e.Message);
                return "Erro na requisição HTTP.";
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro inesperado: " + e.Message);
                return "Erro inesperado.";
            }
        }

    }
}

