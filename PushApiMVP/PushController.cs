using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Mvc;

namespace PushApiMVP
{
    [ApiController]
    [Route("[controller]")]
    public class PushController : ControllerBase
    {
        private readonly IPushSender _pushSender;
        private readonly IDeviceTokenService _deviceTokenService;

        public PushController(IPushSender pushSender, IDeviceTokenService deviceTokenService)
        {
            _pushSender = pushSender;
            _deviceTokenService = deviceTokenService;
        }

        [HttpPost("test")]
        public async Task<IActionResult> TestNotification()
        {
            try
            {
                // This is a test token. You'll need to replace it with a real device token when testing with a real device.
                string testToken = "YOUR_ACTUAL_DEVICE_TOKEN_HERE";
                string messageId = await _pushSender.Send(
                    testToken,
                    "Test Notification",
                    "This is a test notification from your server!",
                    false,
                    new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } }
                );
                return Ok(new { message = "Test notification sent successfully!", messageId = messageId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send test notification: {ex.Message}" });
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] PushSendRequest request)
        {
            if (string.IsNullOrEmpty(request.PushToken))
            {
                return BadRequest(new { message = "Push token is required" });
            }

            try
            {
                string messageId = await _pushSender.Send(
                    request.PushToken,
                    request.NotificationTitle,
                    request.NotificationMessage,
                    request.IsSilent,
                    request.Data ?? new Dictionary<string, string>()
                );
                return Ok(new { message = "Notification sent successfully", messageId = messageId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send notification: {ex.Message}" });
            }
        }

        [HttpPost("sendToAll")]
        public async Task<IActionResult> SendNotificationToAll([FromBody] PushSendRequest request)
        {
            try
            {
                string messageId = await _pushSender.SendToTopic(
                    "All",
                    request.NotificationTitle,
                    request.NotificationMessage,
                    request.IsSilent,
                    request.Data ?? new Dictionary<string, string>()
                );
                return Ok(new { message = "Notification sent successfully", messageId = messageId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send notification: {ex.Message}" });
            }
        }

        [HttpPost("sendToTopic")]
        public async Task<IActionResult> SendNotificationToTopic([FromBody] PushSendRequest request)
        {
            if (string.IsNullOrEmpty(request.Topic))
            {
                return BadRequest(new { message = "Topic is required" });
            }

            try
            {
                string messageId = await _pushSender.SendToTopic(
                    request.Topic,
                    request.NotificationTitle,
                    request.NotificationMessage,
                    request.IsSilent,
                    request.Data ?? new Dictionary<string, string>()
                );
                return Ok(new { message = "Notification sent successfully", messageId = messageId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send notification: {ex.Message}" });
            }
        }

        [HttpPost("sendToCondition")]
        public async Task<IActionResult> SendNotificationToCondition([FromBody] PushSendRequest request)
        {
            if (string.IsNullOrEmpty(request.Condition))
            {
                return BadRequest(new { message = "Condition is required" });
            }

            try
            {
                string messageId = await _pushSender.SendToCondition(
                    request.Condition,
                    request.NotificationTitle,
                    request.NotificationMessage,
                    request.IsSilent,
                    request.Data ?? new Dictionary<string, string>()
                );
                return Ok(new { message = "Notification sent successfully", messageId = messageId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send notification: {ex.Message}" });
            }
        }

        [HttpPost("sendToMultipleDevices")]
        public async Task<IActionResult> SendNotificationToMultipleDevices([FromBody] PushSendRequest request)
        {
            if (request.Tokens == null || request.Tokens.Count == 0)
            {
                return BadRequest(new { message = "Tokens are required" });
            }

            try
            {
                string messageId = await _pushSender.SendToMultipleDevices(
                    request.Tokens,
                    request.NotificationTitle,
                    request.NotificationMessage,
                    request.IsSilent,
                    request.Data ?? new Dictionary<string, string>()
                );
                return Ok(new { message = "Notification sent successfully", messageId = messageId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to send notification: {ex.Message}" });
            }
        }

        [HttpPost("subscribeToTopic")]
        public async Task<IActionResult> SubscribeToTopic([FromBody] SubscribeToTopicRequest request)
        {
            if (string.IsNullOrEmpty(request.PushToken) || string.IsNullOrEmpty(request.Topic))
            {
                return BadRequest(new { message = "Push token and topic are required" });
            }

            try
            {
                string subscriptionId = await _pushSender.SubscribeToTopic(request.PushToken, request.Topic);
                return Ok(new { message = "Subscribed to topic successfully", subscriptionId = subscriptionId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to subscribe to topic: {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterToken([FromBody] TokenRequest request)
        {
            _deviceTokenService.RegisterToken(request.Token);

            // Subscribe the device to the "All" topic
            try
            {
                await _pushSender.SubscribeToTopic(request.Token, "All");
                return Ok(new { message = "Token registered and subscribed to 'All' topic successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to subscribe to 'All' topic: {ex.Message}" });
            }
        }

        [HttpPost("unregister")]
        public IActionResult UnregisterToken([FromBody] TokenRequest request)
        {
            _deviceTokenService.UnregisterToken(request.Token);
            return Ok(new { message = "Token unregistered successfully" });
        }
    }

    public class PushSendRequest
    {
        public string NotificationTitle { get; set; }
        public string NotificationMessage { get; set; }
        public bool IsSilent { get; set; }
        public string? PushToken { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public string? Topic { get; set; }
        public string? Condition { get; set; }
        public List<string>? Tokens { get; set; }
    }

    public class SubscribeToTopicRequest
    {
        public string PushToken { get; set; }
        public string Topic { get; set; }
    }

    public class TokenRequest
    {
        public string Token { get; set; }
    }
}