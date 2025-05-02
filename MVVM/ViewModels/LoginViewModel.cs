using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Repositories;
using Furia_FanHub.MVVM.Services;
using Furia_FanHub.MVVM.Views;

namespace Furia_FanHub.MVVM.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        private readonly Secrets _secrets;
        private readonly FanRepository _fanRepository;
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");

        private PlayerInfo _playerInfo;

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand RegisterCommand { get; }
        public IRelayCommand GoogleLoginCommand { get; }
        public IRelayCommand GuestCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            RegisterCommand = new RelayCommand(async () => await RegisterAsync());
            GoogleLoginCommand = new RelayCommand(async () => await GoogleLoginAsync());
            GuestCommand = new RelayCommand(async () => await GuestLoginAsync());
            _secrets = new Secrets();
            _playerInfo = new PlayerInfo();
            _fanRepository = new FanRepository(dbPath);
        }

        private async Task LoginAsync()
        {
            // Aqui você faria uma validação com banco SQLite ou API
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Email e senha são obrigatórios.", "OK");
                return;
            }

            var fan = await _fanRepository.GetFanByLoginAsync(Email, Password);
            if (fan == null)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha inválidos.", "OK");
                return;
            }

            int id = fan.Id;

            try
            {
                GlobalProperties.FanId = id;
                await Shell.Current.GoToAsync($"//home?id={id}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao fazer login.", "OK");
                Console.WriteLine(ex.Message);
            }
        }

        private async Task RegisterAsync()
        {
            // Navegar para uma página de registro
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        private async Task GoogleLoginAsync()
        {
            try
            {
                var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri("https://accounts.google.com/o/oauth2/v2/auth?" +
                            $"client_id={_secrets.GOOGLE_CLIENT_ID}" +
                            $"&redirect_uri={_secrets.REDIRECT_URI}" +
                            "&response_type=code" +
                            "&scope=openid%20email%20profile" +
                            "&access_type=offline" +
                            "&prompt=consent"),
                    new Uri($"{_secrets.REDIRECT_URI}"));

                var authToken = authResult?.AccessToken;
                if (string.IsNullOrEmpty(authToken))
                    throw new Exception("Token de autenticação inválido.");

                await GetGoogleUserInfoAsync(authToken);
            }
            catch (Exception ex)
            {
                // Trate erros de login
                Console.WriteLine($"Erro ao logar: {ex.Message}");
            }
        }

        private async Task GetGoogleUserInfoAsync(string accessToken)
        {
            var authService = new GoogleAuthService
                (
                clientId: $"{_secrets.GOOGLE_CLIENT_ID}",
                clientSecret: $"{_secrets.GOOGLE_CLIENT_SECRET}",
                redirectUri: $"{_secrets.REDIRECT_URI}"
                );

            try
            {
                var userInfo = await authService.AuthenticateAsync();

                // Exemplo: exibir os dados
                Console.WriteLine($"Usuário autenticado: {userInfo.Name} ({userInfo.Email})");

                // Aqui você pode salvar em memória ou enviar para seu banco de dados interno
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha no login: {ex.Message}");
            }
        }

        private async Task GuestLoginAsync()
        {
            GlobalProperties.FanId = 0;
            await Shell.Current.GoToAsync("//home?id=0");
        }
    }
}
