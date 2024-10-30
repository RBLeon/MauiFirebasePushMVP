using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Analytics;
using Shiny;
using Shiny.Push;
#if ANDROID
using Android.App;
using Plugin.Firebase.Core.Platforms.Android; 
#endif
#if IOS 
using Plugin.Firebase.Core.Platforms.iOS;
#endif 

namespace PushReceiverMVP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseShiny()
                .RegisterFirebaseServices() 
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            var apiUrl = "https://10.0.2.2:7042"; // Use this for Android emulator
            // var apiUrl = "https://localhost:7042"; // Use this for iOS simulator or physical device
#else
        var apiUrl = "https://10.0.2.2:7042"; 
        // change this to your production URL
        // currently does not matter where you set it, due to certificate issues you need to manually retrieve the token
        // after that you need to register it in the swagger window that opens, and then you can hit the test all button (as it auto subscribes to the topic)
        // Or you can choose to hardcode the token in the api where there is now "YOUR_TOKEN" in the code
        
#endif
#if ANDROID
            var devSslHelper = new DevHttpsConnectionHelper();
#endif
#if DEBUG && ANDROID
            HttpClient client = new HttpClient(devSslHelper.GetPlatformMessageHandler())
            {
                BaseAddress = new Uri(apiUrl)
            };
#else
        HttpClient client = new HttpClient { BaseAddress = new Uri(apiUrl) };
#endif

            builder.Services.AddSingleton(client);

            builder.Logging.AddDebug();
            
            builder.Services.AddPushFirebaseMessaging<PushDelegate>(
                new FirebaseConfiguration(
                    false,
#if IOS
                    builder.Configuration["Firebase:AppleAppId"],
#elif ANDROID
                    builder.Configuration["Firebase:AndroidAppId"],
#endif
                    builder.Configuration["Firebase:ProjectNumber"],
                    builder.Configuration["Firebase:ProjectId"],
                    builder.Configuration["Firebase:ApiKey"]
#if ANDROID
                    , DefaultChannel
#endif
                )
            );

            return builder.Build();
        }
        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events => {
        #if IOS
                events.AddiOS(iOS => iOS.WillFinishLaunching((app, launchOptions) => {
                    CrossFirebase.Initialize();
                    return true;
                }));
        #elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                {
                    try
                    {
                        CrossFirebase.Initialize(activity);
                        FirebaseAnalyticsImplementation.Initialize(activity);
                        CrossFirebaseAnalytics.Current.IsAnalyticsCollectionEnabled = true;
                        
                        // Ensure Google Play Services are available
                        var availability = Android.Gms.Common.GoogleApiAvailability.Instance;
                        var result = availability.IsGooglePlayServicesAvailable(activity);
                        if (result != Android.Gms.Common.ConnectionResult.Success)
                        {
                            if (availability.IsUserResolvableError(result))
                            {
                                availability.ShowErrorNotification(activity, result);
                            }
                            throw new Exception($"Google Play Services not available: {result}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Firebase Init Error: {ex}");
                        throw; // Or handle appropriately
                    }
                }));
#endif
            });
    
            builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);
    
            return builder;
        }
        
#if ANDROID
        static NotificationChannel DefaultChannel => new(
            "default_channel_id",
            "Default Channel",
            NotificationImportance.Default
        )
        {
            LockscreenVisibility = NotificationVisibility.Public
        };
#endif
    }
}
