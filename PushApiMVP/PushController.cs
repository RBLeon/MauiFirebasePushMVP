using Microsoft.AspNetCore.Mvc;

namespace PushApiMVP
{
    [ApiController]
    [Route("[controller]")]
    public class PushController : ControllerBase
    {
        private readonly IPushSender _pushSender;

        public PushController(IPushSender pushSender)
        {
            _pushSender = pushSender;
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
    }

    public class PushSendRequest
    {
        public string NotificationTitle { get; set; }
        public string NotificationMessage { get; set; }
        public bool IsSilent { get; set; }
        public string PushToken { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}