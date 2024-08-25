namespace PushApiMVP;

public interface IPushSender
{
    Task<string> Send(string token, string title, string message, bool silent, Dictionary<string, string> data);
    Task<string> SendToTopic(string topic, string title, string message, bool silent, Dictionary<string, string> data);
    Task<string> SendToCondition(string condition, string title, string message, bool silent, Dictionary<string, string> data);
    Task<string> SendToMultipleDevices(List<string> tokens, string title, string body, bool silent, Dictionary<string, string> data);
    Task<string> SubscribeToTopic(string token, string topic);
}