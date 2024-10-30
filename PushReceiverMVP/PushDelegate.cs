using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Plugin.Firebase.Analytics;
using Shiny.Push;
#if IOS
using UserNotifications;
#endif

namespace PushReceiverMVP
{
    public partial class PushDelegate : IPushDelegate
    {
        private readonly ILogger<PushDelegate> _logger;
        private readonly HttpClient _httpClient;
        private readonly IFirebaseAnalytics _firebaseAnalytics;

        public PushDelegate(
            ILogger<PushDelegate> logger, 
            HttpClient httpClient,
            IFirebaseAnalytics firebaseAnalytics)
        {
            _logger = logger;
            _httpClient = httpClient;
            _firebaseAnalytics = firebaseAnalytics;
            _firebaseAnalytics.IsAnalyticsCollectionEnabled = true;
        }

        public Task OnEntry(PushNotification notification)
        {
            _logger.LogInformation("Notification tapped");
            return Task.CompletedTask;
        }

        public Task OnReceived(PushNotification notification)
        {
            _logger.LogInformation("Notification received");
            return Task.CompletedTask;
        }

        public async Task OnNewToken(string token)
        {
            _logger.LogInformation("New push token: {Token}", token);
            
            // Track token refresh
            _firebaseAnalytics.LogEvent("fcm_token_refresh", new Dictionary<string, object>
            {
                { "token_status", "new" }
            });
            
            await RegisterTokenWithServer(token);
        }

        public async Task OnUnRegistered(string token)
        { 
            _logger.LogInformation("Push token unregistered: {Token}", token);
            
            // Track token unregistration
            _firebaseAnalytics.LogEvent("fcm_token_refresh", new Dictionary<string, object>
            {
                { "token_status", "unregistered" }
            });
            
            await UnregisterTokenWithServer(token);
        }

        private async Task RegisterTokenWithServer(string token)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(new { Token = token }), 
                    Encoding.UTF8, 
                    "application/json"
                );
                var response = await _httpClient.PostAsync("push/register", content);
                response.EnsureSuccessStatusCode();
                
                _logger.LogInformation("Token registered successfully with server and subscribed to 'All' topic");
                
                // Track successful registration
                _firebaseAnalytics.LogEvent("token_server_registration", new Dictionary<string, object>
                {
                    { "status", "success" }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register token with server");
                
                // Track failed registration
                _firebaseAnalytics.LogEvent("token_server_registration", new Dictionary<string, object>
                {
                    { "status", "failed" },
                    { "error", ex.Message }
                });
            }
        }

        private async Task UnregisterTokenWithServer(string token)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(new { Token = token }), 
                    Encoding.UTF8, 
                    "application/json"
                );
                var response = await _httpClient.PostAsync("push/unregister", content);
                response.EnsureSuccessStatusCode();
                
                _logger.LogInformation("Token unregistered successfully from server");
                
                // Track successful unregistration
                _firebaseAnalytics.LogEvent("token_server_unregistration", new Dictionary<string, object>
                {
                    { "status", "success" }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to unregister token from server");
                
                // Track failed unregistration
                _firebaseAnalytics.LogEvent("token_server_unregistration", new Dictionary<string, object>
                {
                    { "status", "failed" },
                    { "error", ex.Message }
                });
            }
        }

#if IOS
        public UNAuthorizationOptions NotificationOptions =>
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
#endif
    }
}