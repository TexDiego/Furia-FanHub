using CommunityToolkit.Maui.Views;

namespace Furia_FanHub.MVVM.Views;

public partial class CustomPopUp : Popup
{
	public CustomPopUp()
	{
		InitializeComponent();
	}

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.Close(false);
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {
        if (!TermsCheckBox.IsChecked)
        {
            await App.Current.MainPage.DisplayAlert("Atenção", "Você deve aceitar os termos de uso para continuar.", "OK");
            return;
        }

        this.Close(true);
    }
}