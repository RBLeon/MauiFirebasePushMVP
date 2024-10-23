using System.Net.Security;

#if ANDROID
using Android.Net;
using Xamarin.Android.Net;

namespace PushReceiverMVP
{
    public class DevHttpsConnectionHelper
    {
        public DevHttpsConnectionHelper()
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
        }

        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if DEBUG
            return new IgnoreSSLAndroidClientHandler();
#else
            return null;
#endif
        }
    }

    public class IgnoreSSLAndroidClientHandler : Xamarin.Android.Net.AndroidMessageHandler
    {
        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
            => new CustomHostnameVerifier();

        private class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
        {
            public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session)
            {
                return true;
            }
        }
    }
}
#endif