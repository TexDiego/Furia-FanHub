using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.ViewModels;

namespace Furia_FanHub.MVVM.Views;

public partial class ProfilePage : ContentPage
{
	private readonly ProfileViewModel _viewModel;

    public ProfilePage()
	{
		InitializeComponent();
        _viewModel = new ProfileViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProfileViewModel vm)
        {
            vm.RefreshData();
        }
    }

    private void SettingsButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//settings");
    }
}