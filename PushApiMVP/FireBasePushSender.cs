using System.Text.Json;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace PushApiMVP;

public class FirebasePushSender : IPushSender
{
    private readonly FirebaseApp app;
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
            var cred = GoogleCredential.FromServiceAccountCredential(ServiceAccountCredential.FromServiceAccountData(stream));

            this.app = FirebaseApp.Create(new AppOptions
            {
                Credential = cred
            });
            this.messaging = FirebaseMessaging.GetMessaging(this.app);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task Send(string token, string title, string message, bool silent, Dictionary<string, string> data)
    {
        try
        {
            //var fcmMessage = new Message
            //{
            //    Data = data,
            //    Token = token,
            //    Apns = new ApnsConfig
            //    {
            //        Aps = new Aps
            //        {
            //            ContentAvailable = true
            //        }
            //    }
            //};
            //if (!silent)
            //{
            //    fcmMessage.Notification = new Notification
            //    {
            //        Title = title,
            //        Body = message
            //    };
            //    fcmMessage.Android = new AndroidConfig
            //    {
            //        Notification = new AndroidNotification
            //        {
            //            Title = title,
            //            Body = message,
            //            ChannelId = "YOUR_DEFAULT_CHANNEL_ID" // Replace with your actual default channel ID
            //        }
            //    };
            //}
            var fcmMessage = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = message
                },
                Topic = "topic",
            };
            var response = await this.messaging.SendAsync(fcmMessage);
            Console.WriteLine("Successfully sent message: " + response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}