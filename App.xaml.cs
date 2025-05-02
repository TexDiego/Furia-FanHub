using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Services;

namespace Furia_FanHub
{
    public partial class App : Application
    {
        public App()
        {
#if ANDROID
        // Aplica o tema ao iniciar o app
        ApplyAndroidStatusBarTheme();

        // Escuta mudanças no tema do sistema
        Application.Current.RequestedThemeChanged += (s, e) =>
            {
                bool isDark = e.RequestedTheme == AppTheme.Dark;
                var activity = Platform.CurrentActivity as MainActivity;
                activity?.RunOnUiThread(() => activity?.SetStatusBarTheme(isDark));
            };

            MainPage?.Handler?.UpdateValue(nameof(MainPage.BackgroundColor));
#endif
            InitializeComponent();
            MainPage = new AppShell();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(CustomEntry), (handler, view) =>
            {

#if __ANDROID__

                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

#endif
            });

            Shell.Current.GoToAsync("//Login");
        }

#if ANDROID
    private void ApplyAndroidStatusBarTheme()
    {
        var activity = Platform.CurrentActivity as MainActivity;
        bool isDarkNow = Application.Current.RequestedTheme == AppTheme.Dark;
        activity?.RunOnUiThread(() => activity?.SetStatusBarTheme(isDarkNow));
    }
#endif
    }
}
