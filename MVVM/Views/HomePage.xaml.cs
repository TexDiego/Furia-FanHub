using Furia_FanHub.MVVM.ViewModels;

namespace Furia_FanHub.MVVM.Views;

public partial class HomePage : ContentPage, IQueryAttributable
{
    private bool _isGuest = true;
    private int id = 0;
    private HomeViewModel _viewModel;

    public HomePage()
    {
        InitializeComponent();
    }

    private void UserLogin()
    {
        try
        {
            _viewModel = new HomeViewModel(id);
            BindingContext = _viewModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao inicializar o ViewModel: {ex.Message}");
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            if (query.TryGetValue("id", out var idValue) && int.TryParse(idValue.ToString(), out var parsedId))
            {
                id = parsedId;
                _isGuest = id > 0 ? false : true;
                UserLogin();
            }

            if (_isGuest)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Você está logado como visitante. Para acessar todas as funcionalidades, faça login.", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao carregar os dados do usuário.", "OK");
            Console.WriteLine(ex.Message);
        }
    }

    private async void XButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Launcher.OpenAsync("https://x.com/FURIA");
        }
        catch
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao abrir o link.", "OK");
        }
    }

    private void InstagramButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            Launcher.OpenAsync("https://www.instagram.com/furiagg/");
        }
        catch
        {
            App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao abrir o link.", "OK");
        }
    }

    private void whatsappButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            Launcher.OpenAsync("https://wa.me/5511993404466");
        }
        catch
        {
            App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao abrir o link.", "OK");
        }
    }
}