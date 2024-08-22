using Microsoft.Extensions.Logging;
using Shiny;

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
//            builder.Services.AddPushFirebaseMessaging<PushReceiverMVP.PushDelegate>(
//#if ANDROID
//                new Shiny.Push.FirebaseConfig(
//                    false, // false to ignore embedded configuration
//                    "YOUR_APP_ID",
//                    "YOUR_SENDER_ID",
//                    "YOUR_PROJECT_ID",
//                    "YOUR_API_KEY"
//                )
//#endif
//            );

            return builder.Build();
        }
    }
}
