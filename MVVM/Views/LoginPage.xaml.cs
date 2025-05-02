using Furia_FanHub.MVVM.ViewModels;

namespace Furia_FanHub.MVVM.Views;

public partial class LoginPage : ContentPage
{
	private LoginViewModel _viewModel;

    public LoginPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel = new LoginViewModel();
        BindingContext = _viewModel;
        PasswordEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
    }
}