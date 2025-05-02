using Android.App;
using Android.Content;
using Android.Content.PM;

namespace Furia_FanHub.Platforms.Android
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported =true)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "furiafanhub",
        DataHost = "oauth2redirect",
        AutoVerify = true)]
    public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    {
    }
}
