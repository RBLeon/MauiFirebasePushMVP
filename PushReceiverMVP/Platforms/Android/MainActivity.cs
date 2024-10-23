using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Firebase;
using Firebase.Analytics;

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
        protected override void OnCreate(Bundle? savedInstanceState) 
        { 
            base.OnCreate(savedInstanceState); 
 
            Window?.AddFlags(WindowManagerFlags.TranslucentStatus); 
            Window?.SetStatusBarColor(Android.Graphics.Color.Transparent); 
             
            // Initialize Firebase 
            FirebaseApp.InitializeApp(this); 
             
            // Get Firebase Analytics instance 
            FirebaseAnalytics.GetInstance(this); 
        } 
    }
}
