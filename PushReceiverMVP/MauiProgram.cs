using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Push;
#if ANDROID
using Android.App;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
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

#if ANDROID
        static NotificationChannel DefaultChannel => new(
            "default_channel",
            "Default Channel",
            NotificationImportance.Default
        )
        {
            LockscreenVisibility = NotificationVisibility.Public
        };
#endif
    }
}
