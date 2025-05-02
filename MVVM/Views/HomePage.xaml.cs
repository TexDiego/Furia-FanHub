using Furia_FanHub.MVVM.Repositories;
using Furia_FanHub.MVVM.ViewModels;

namespace Furia_FanHub.MVVM.Views;

public partial class HomePage : ContentPage, IQueryAttributable
{
    private bool _isGuest = true;
    private int id = 0;
    private HomeViewModel _viewModel;
    private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");
    private FanRepository _fanRepository;

    public HomePage()
    {
        InitializeComponent();
    }

    private void UserLogin()
    {
        try
        {
            _fanRepository = new FanRepository(dbPath);
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
}