using System;
using Foundation;
using Java.Interop;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

#if ANDROID
using Android.Runtime;
#elif IOS
using UIKit;
#endif

namespace PushReceiverMVP
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
            => global::Shiny.Hosting.Host.Lifecycle.OnRegisteredForRemoteNotifications(deviceToken);

        [Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
        public void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
            => global::Shiny.Hosting.Host.Lifecycle.OnFailedToRegisterForRemoteNotifications(error);

        [Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
            => global::Shiny.Hosting.Host.Lifecycle.OnDidReceiveRemoteNotification(userInfo, completionHandler);

    }
}
