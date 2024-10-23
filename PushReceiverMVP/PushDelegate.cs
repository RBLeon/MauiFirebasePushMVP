using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
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

        public PushDelegate(ILogger<PushDelegate> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
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
            await RegisterTokenWithServer(token);
        }

        public async Task OnUnRegistered(string token)
        { 
            _logger.LogInformation("Push token unregistered: {Token}", token);
            await UnregisterTokenWithServer(token);
        }

        private async Task RegisterTokenWithServer(string token)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { Token = token }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("push/register", content);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Token registered successfully with server and subscribed to 'All' topic");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register token with server");
            }
        }

        private async Task UnregisterTokenWithServer(string token)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { Token = token }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("push/unregister", content);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Token unregistered successfully from server");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to unregister token from server");
            }
        }

#if IOS
        public UNAuthorizationOptions NotificationOptions =>
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
#endif
    }
}