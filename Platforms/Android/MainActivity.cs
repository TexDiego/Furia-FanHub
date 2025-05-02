using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Furia_FanHub
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        public void SetStatusBarTheme(bool isDark)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var window = Window;
                window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);

                var color = isDark ? Android.Graphics.Color.Black : Android.Graphics.Color.White;
                window.SetStatusBarColor(color);

                var decor = window.DecorView;
                decor.SystemUiVisibility = isDark ? 0 : (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the status bar theme based on the current theme
            var isDarkTheme = App.Current.RequestedTheme == AppTheme.Dark;
            SetStatusBarTheme(isDarkTheme);

            Instance = this;
        }
    }
}
