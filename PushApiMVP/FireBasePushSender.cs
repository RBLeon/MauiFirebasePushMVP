using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace PushApiMVP;

public class FirebasePushSender : IPushSender
{
    private readonly FirebaseMessaging messaging;

    public FirebasePushSender(IConfiguration configuration)
    {
        try
        {
            var googleConfig = configuration.GetSection("Push:Google").Get<GoogleConfiguration>();
            var json = JsonSerializer.Serialize(new
            {
                type = "service_account",
                project_id = googleConfig.ProjectId,
                private_key_id = googleConfig.PrivateKeyId,
                private_key = googleConfig.PrivateKey,
                client_email = googleConfig.ClientEmail,
                client_id = googleConfig.ClientId,
                auth_uri = "https://accounts.google.com/o/oauth2/auth",
                token_uri = "https://oauth2.googleapis.com/token",
                auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
                client_x509_cert_url = googleConfig.ClientX509CertUrl
            });
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
            var credential = GoogleCredential.FromServiceAccountCredential(ServiceAccountCredential.FromServiceAccountData(stream));

            FirebaseApp app = FirebaseApp.Create(new AppOptions
            {
                Credential = credential
            });
            this.messaging = FirebaseMessaging.GetMessaging(app);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing FirebasePushSender: {ex}");
            throw;
        }
    }

    public async Task<string> Send(string token, string title, string body, bool silent, Dictionary<string, string> data)
    {
        try
        {
            var message = new Message
            {
                Token = token,
                Data = data,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = title,
                        Body = body,
                        ChannelId = "default_channel_id",
                        ClickAction = "SHINY_PUSH_NOTIFICATION_CLICK"
                    }
                }
            };

            if (silent)
            {
                message.Android.Notification = null;
            }

            string response = await messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending Firebase message: {ex}");
            throw;
        }
    }

    public async Task<string> SendToTopic(string topic, string title, string body, bool silent, Dictionary<string, string> data)
    {
        try
        {
            var message = new Message
            {
                Topic = topic,
                Data = data,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = title,
                        Body = body,
                        ChannelId = "default_channel_id",
                        ClickAction = "SHINY_PUSH_NOTIFICATION_CLICK"
                    }
                }
            };

            if (silent)
            {
                message.Android.Notification = null;
            }

            string response = await messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending Firebase message: {ex}");
            throw;
        }
    }

    public async Task<string> SendToCondition(string condition, string title, string body, bool silent, Dictionary<string, string> data)
    {
        try
        {
            var message = new Message
            {
                Condition = condition,
                Data = data,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = title,
                        Body = body,
                        ChannelId = "default_channel_id",
                        ClickAction = "SHINY_PUSH_NOTIFICATION_CLICK"
                    }
                }
            };

            if (silent)
            {
                message.Android.Notification = null;
            }

            string response = await messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending Firebase message: {ex}");
            throw;
        }
    }

    public async Task<string> SendToMultipleDevices(List<string> tokens, string title, string body, bool silent, Dictionary<string, string> data)
    {
        try
        {
            var message = new MulticastMessage
            {
                Tokens = tokens,
                Data = data,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = title,
                        Body = body,
                        ChannelId = "default_channel_id",
                        ClickAction = "SHINY_PUSH_NOTIFICATION_CLICK"
                    }
                }
            };

            if (silent)
            {
                message.Android.Notification = null;
            }

            BatchResponse response = await messaging.SendEachForMulticastAsync(message);
            Console.WriteLine($"Successfully sent message to {response.SuccessCount} devices");
            return response.SuccessCount.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending Firebase message: {ex}");
            throw;
        }
    }

    // subscribe to a topic
    public async Task<string> SubscribeToTopic(string token, string topic)
    {
        try
        {
            await messaging.SubscribeToTopicAsync(new List<string> { token }, topic);
            Console.WriteLine($"Successfully subscribed to topic {topic}");
            return topic;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error subscribing to topic: {ex}");
            throw;
        }
    }
}