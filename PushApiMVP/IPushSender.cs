namespace PushApiMVP;

public interface IPushSender
{
    Task Send(string token, string title, string message, bool silent, Dictionary<string, string> data);
}