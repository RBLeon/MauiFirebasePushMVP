using Shiny.Hosting;
using Shiny.Push;
using Shiny;

namespace PushReceiverMVP
{
    public partial class App : Application
    {
        public App()
        {
            //InitializeComponent();

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
            var result = await push.RequestAccess();
            if (result.Status == AccessState.Available)
            {
                // good to go
                var token = result.RegistrationToken;
                // you should send this to your server with a userId attached if you want to do custom work
                Console.WriteLine($"Push token: {token}");
            }
        }
    }
}
