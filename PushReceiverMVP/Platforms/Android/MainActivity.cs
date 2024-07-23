using Android.App;
using Android.Content.PM;
using Android.OS;

namespace PushReceiverMVP
{
    [Activity(Theme = "@style/Maui.SplashTheme", 
        MainLauncher = true, 
        LaunchMode = LaunchMode.SingleTop, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(
        new[] {
            Shiny.ShinyPushIntents.NotificationClickAction
        },
        Categories = new[] {
            "android.intent.category.DEFAULT"
        }
    )]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
