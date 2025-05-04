using Furia_FanHub.MVVM.Models.Services;
using Furia_FanHub.MVVM.ViewModels;
using System.Text.RegularExpressions;

namespace Furia_FanHub.MVVM.Views;

public partial class ConfigsPage : ContentPage
{
	private readonly ConfigsViewModel _viewModel;
    private readonly CEPService _cepService;

    public ConfigsPage()
	{
		InitializeComponent();
        _cepService = new CEPService();
        BindingContext = _viewModel = new ConfigsViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.CarregarInteressesUsuarioAsync();
    }

    private async void SearchCEPButton_Clicked(object sender, EventArgs e)
    {
        string cep = CEPEntry.Text;

        if (!ValidarCep(cep))
        {
            await this.DisplayAlert("Erro", "CEP inválido. O CEP deve conter 8 dígitos.", "OK");
            return;
        }

        try
        {
            var endereco = await _cepService.BuscarEnderecoPorCep(cep);

            if (endereco == null || string.IsNullOrEmpty(endereco.Rua))
            {
                await this.DisplayAlert("Erro", "Endereço não encontrado. Verifique o CEP.", "OK");
                return;
            }

            Logradouro.Text = endereco.Rua;
            Bairro.Text = endereco.Bairro;
            Cidade.Text = endereco.Cidade;
            UF.Text = endereco.Estado;
            Pais.Text = "Brasil";

        }
        catch (HttpRequestException ex)
        {
            await this.DisplayAlert("Erro", "Erro ao buscar o endereço. Verifique sua conexão com a internet.", "OK");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Erro", "Ocorreu um erro inesperado. Tente novamente mais tarde.", "OK");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    private bool ValidarCep(string cep)
    {
        return !string.IsNullOrWhiteSpace(cep) && cep.Length == 8 && cep.All(char.IsDigit);
    }

    private void Entry_CEP_TextChanged(object sender, TextChangedEventArgs e)
    {
        var regex = new Regex("^[0-9]*$");

        if (!string.IsNullOrEmpty(e.NewTextValue) && !regex.IsMatch(e.NewTextValue))
        {
            // Reverte para o texto anterior caso contenha caracteres inválidos
            ((Entry)sender).Text = e.OldTextValue;
        }
    }
}