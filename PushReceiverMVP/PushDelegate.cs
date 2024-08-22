using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Push;
//using PushReceiverMVP.Services;
#if IOS
using UserNotifications;
#endif

namespace PushReceiverMVP
{
    public partial class PushDelegate : IPushDelegate
    {
        private readonly ILogger<PushDelegate> _logger;
        //private readonly INotificationService _notificationService;

        public PushDelegate(ILogger<PushDelegate> logger)
        {
            _logger = logger;
            //_notificationService = notificationService;
        }

        public async Task OnEntry(PushNotification notification)
        {
            _logger.LogInformation("Notification tapped");
            //await _notificationService.HandleNotificationEntry(notification);
        }

        public async Task OnReceived(PushNotification notification)
        {
            _logger.LogInformation("Notification received");
            //await _notificationService.HandleNotificationReceived(notification);
        }

        public async Task OnNewToken(string token)
        {
            _logger.LogInformation("New push token: {Token}", token);
            //await _notificationService.SendTokenToServer(token);
        }

        public async Task OnUnRegistered(string token)
        {
            _logger.LogInformation("Push token unregistered: {Token}", token);
            //await _notificationService.RemoveTokenFromServer(token);
        }

#if IOS
        public UNAuthorizationOptions NotificationOptions =>
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
#endif
    }
}