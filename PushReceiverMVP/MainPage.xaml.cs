using System;
using Microsoft.Maui.Controls;
using Shiny;
using Shiny.Hosting;
using Shiny.Push;

namespace PushReceiverMVP
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var push = Host.Current.Services.GetService<IPushManager>();
            if (push != null)
            {
                var result = await push.RequestAccess();
                if (result.Status == AccessState.Available)
                {
                    CounterBtn.Text = $"Token: {result.RegistrationToken}";
                }
                else
                {
                    CounterBtn.Text = "Push notifications not available";
                }
            }
            else
            {
                CounterBtn.Text = "Push service not found";
            }

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
