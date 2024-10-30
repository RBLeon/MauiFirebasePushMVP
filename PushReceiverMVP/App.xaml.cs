using Shiny.Hosting;
using Shiny.Push;
using Shiny;

namespace PushReceiverMVP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await CheckPushPermission();
        }

        private async Task CheckPushPermission()
        {
            var push = Host.Current.Services.GetService<IPushManager>();
            var pushDelegate = Host.Current.Services.GetService<IPushDelegate>();
            var result = await push.RequestAccess();
            if (result.Status == AccessState.Available)
            {
                // good to go
                var token = result.RegistrationToken;
                
                Console.WriteLine($"Push token: {token}");
                
                // subscribe to All topic by default (read that this registration might be lost sporadically) https://stackoverflow.com/questions/67283587/firebase-cloud-messaging-reports-wrong using the pushdelegate
                await pushDelegate.OnNewToken(token);
            }
        }
    }
}
