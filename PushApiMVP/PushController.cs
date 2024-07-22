using Microsoft.AspNetCore.Mvc;

namespace PushApiMVP
{
    [ApiController]
    [Route("[controller]")]
    public class PushController(IPushSender pushSender) : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] PushSendRequest request)
        {
            await pushSender.Send(request.PushToken, request.NotificationTitle, request.NotificationMessage, request.IsSilent, request.Data);
            return Ok();
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