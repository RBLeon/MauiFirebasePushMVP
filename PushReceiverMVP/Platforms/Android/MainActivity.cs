using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Plugin.Firebase.Analytics;
using Plugin.Firebase.Core.Platforms.Android;
using Shiny;

namespace PushReceiverMVP
{
    [Activity(
        Theme = "@style/Maui.SplashTheme", 
        MainLauncher = true, 
        ConfigurationChanges = 
            ConfigChanges.ScreenSize | 
            ConfigChanges.Orientation | 
            ConfigChanges.UiMode | 
            ConfigChanges.ScreenLayout | 
            ConfigChanges.SmallestScreenSize | 
            ConfigChanges.Density
    )]
    [IntentFilter(
        new[] { 
            ShinyPushIntents.NotificationClickAction 
        },    
        Categories = new[] { 
            global::Android.Content.Intent.CategoryDefault
        }
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState) 
        { 
            base.OnCreate(savedInstanceState); 
 
            Window?.AddFlags(WindowManagerFlags.TranslucentStatus); 
            Window?.SetStatusBarColor(Android.Graphics.Color.Transparent);
        }
    }
}
