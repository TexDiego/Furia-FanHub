using Furia_FanHub.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Furia_FanHub.MVVM.Services
{
    public class GoogleAuthService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        public GoogleAuthService(string clientId, string clientSecret, string redirectUri)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUri = redirectUri;
        }

        public async Task<GoogleUserInfo> AuthenticateAsync()
        {
            try
            {
                // 1. Abrir autenticação no navegador
                var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri($"https://accounts.google.com/o/oauth2/v2/auth?" +
                            $"client_id={_clientId}" +
                            $"&redirect_uri={_redirectUri}" +
                            "&response_type=code" +
                            "&scope=openid%20email" +
                            "&access_type=offline" +
                            "&prompt=consent"),
                    new Uri(_redirectUri));

                var code = authResult?.Properties["code"];

                if (string.IsNullOrEmpty(code))
                    throw new Exception("Código de autorização inválido.");

                // 2. Trocar o code por Access Token
                var tokenResponse = await ExchangeCodeForTokenAsync(code);

                if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                    throw new Exception("Falha ao obter access token.");

                // 3. Buscar informações do usuário
                var userInfo = await FetchUserInfoAsync(tokenResponse.AccessToken);

                return userInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na autenticação: {ex.Message}");
                throw;
            }
        }

        private async Task<(string AccessToken, string IdToken)> ExchangeCodeForTokenAsync(string code)
        {
            var client = new HttpClient();

            var values = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "redirect_uri", _redirectUri },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao trocar código por token: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            var tokenResult = JsonSerializer.Deserialize<GoogleTokenResponse>(json);

            return (tokenResult.AccessToken, tokenResult.IdToken);
        }


        private async Task<GoogleUserInfo> FetchUserInfoAsync(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar informações do usuário: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(json);

            return userInfo;
        }
    }
}
