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
                client_x509_cert_url = googleConfig.ClientX509CertUrl,
                auth_uri = "https://accounts.google.com/o/oauth2/auth",
                token_uri = "https://oauth2.googleapis.com/token",
                auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
                universe_domain = "googleapis.com"
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

    public async Task Send(string token, string title, string body, bool silent, Dictionary<string, string> data)
    {
        try
        {
            var message = new Message
            {
                Token = token,
                Data = data,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        ChannelId = "default_channel_id" // Make sure this matches the channel ID in your Android app
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Alert = new ApsAlert
                        {
                            Title = title,
                            Body = body
                        },
                        Badge = 1,
                        Sound = "default"
                    }
                }
            };

            if (silent)
            {
                message.Android.Notification = null;
                message.Apns.Aps.ContentAvailable = true;
                message.Apns.Aps.Alert = null;
            }

            string response = await messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending Firebase message: {ex}");
            throw;
        }
    }
}