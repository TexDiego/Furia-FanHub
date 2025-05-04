using CommunityToolkit.Maui.Views;
using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Models.Services;
using Furia_FanHub.MVVM.Repositories;
using Furia_FanHub.MVVM.ViewModels;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Furia_FanHub.MVVM.Views;

public partial class RegisterPage : ContentPage
{
	private readonly CadastroUsuarioViewModel _viewModel;
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly CEPService _cepService;
    private FanRepository _fanRepository;

    private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");

    public ValidatableObject<string> Email;
    private readonly CPFValidationRule _cpfValidationRule = new();

    private AppTheme _currentTheme;
    public AppTheme CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                OnPropertyChanged(nameof(CurrentTheme));
            }
        }
    }

    public RegisterPage()
	{
		InitializeComponent();
        _viewModel = new CadastroUsuarioViewModel();
        _cepService = new CEPService();
        _fanRepository = new FanRepository(dbPath);
        BindingContext = _viewModel;

        RequestTheme();
        SetUpDatePicker();
        SetUpEmailValidation();        
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("//Login");
        return base.OnBackButtonPressed();
    }

    private void RequestTheme()
    {
        CurrentTheme = Application.Current.RequestedTheme;

        Application.Current.RequestedThemeChanged += (s, e) =>
        {
            CurrentTheme = e.RequestedTheme;
        };
    }

    private void SetUpDatePicker()
    {
        BirthDatePicker.MaximumDate = DateTime.Today;
        BirthDatePicker.MinimumDate = DateTime.Now.AddYears(-120);
        BirthDatePicker.Date = DateTime.Today;
    }

    private void SetUpEmailValidation()
    {
        Email = new ValidatableObject<string>();
        Email.Validations.Add(new RegexValidationRule
        {
            Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ValidationMessage = "E-mail inválido"
        });
    }

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
    // Método para buscar o endereço pelo CEP
    #region Buscar Endereço
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
    #endregion

    private void ShowPassword1_Clicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        if (_currentTheme == AppTheme.Dark)
            ShowPassword1.Source = PasswordEntry.IsPassword ? "eyehiddenwhite" : "eyeopenwhite";
        else
            ShowPassword1.Source = PasswordEntry.IsPassword ? "eyehiddenblack" : "eyeopenblack";
    }

    private async void RegisterUser_Clicked(object sender, EventArgs e)
    {
        try
        {
            FanProfile fanProfile = FanProfile();

            if (!ValidateEmail() || !ValidateCPF(fanProfile.CPF) || VerifyNullOrWhiteSpaces() || await UserExists()) return;

            bool PolicyAndPrivacyAwareness = (bool)(await this.ShowPopupAsync(new CustomPopUp()));
            if (!PolicyAndPrivacyAwareness) return;

            await _fanRepository.SaveFanAsync(fanProfile);

            await App.Current.MainPage.DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");

            await Shell.Current.GoToAsync("//login");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao cadastrar o usuário. Tente novamente.", "OK");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.Message);
        }
    }

    private async Task<bool> UserExists()
    {
        try
        {
            var fanEmail = await _fanRepository.GetFanByEmailAsync(EmailEntry.Text);
            var fanCpf = await _fanRepository.GetFanByCPFAsync(CpfEntry.Text);

            bool userExists = fanEmail != null || fanCpf != null;

            if (fanEmail != null)
                await App.Current.MainPage.DisplayAlert("Ops", "Já existe um usuário cadastrado com esse e-mail.", "OK");
            if (fanCpf != null)
                await App.Current.MainPage.DisplayAlert("Ops", "Já existe um usuário cadastrado com esse CPF.", "OK");

            return userExists;
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao verificar o usuário. Tente novamente.", "OK");
            Console.WriteLine(ex.Message);

            return false;
        }
    }

    private FanProfile FanProfile()
    {
        return new FanProfile
            {
                Email = EmailEntry.Text,
                NomeCompleto = NomeEntry.Text,
                DataNascimento = BirthDatePicker.Date,
                CPF = CpfEntry.Text,
                Senha = PasswordEntry.Text,
                CEP = CEPEntry.Text,
                Logradouro = Logradouro.Text,
                Bairro = Bairro.Text,
                Cidade = Cidade.Text,
                UF = UF.Text,
                Pais = Pais.Text,
                Interesses = string.Join(", ", _viewModel.InteressesDisponiveis.Where(i => i.IsSelected).Select(i => i.Name))
            };
    }

    private bool VerifyNullOrWhiteSpaces()
    {
        bool verification = (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text)
            || string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(CpfEntry.Text));

        if (verification) App.Current.MainPage.DisplayAlert("Ops", "Preencha todos os campos obrigatórios.", "OK");
        return verification;
    }

    private bool ValidateEmail()
    {
        var regex = new RegexValidationRule
        {
            Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ValidationMessage = "E-mail inválido"
        };

        if (!regex.Check(EmailEntry.Text))
            App.Current.MainPage.DisplayAlert("Ops", regex.ValidationMessage, "OK");

        return regex.Check(EmailEntry.Text);
    }

    private bool ValidateCPF(string cpf)
    {
        if (!_cpfValidationRule.Check(cpf)) App.Current.MainPage.DisplayAlert("Ops", _cpfValidationRule.ValidationMessage, "OK");
        return _cpfValidationRule.Check(cpf);
    }
}