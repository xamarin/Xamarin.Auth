using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Xamarin.Auth.DeviceTests
{
    public class WebViewConfiguration_Test
    {
        [Fact]
        public void Can_Set_Properties()
        {
#if __IOS__
            WebViewConfiguration.IOS.IsUsingWKWebView = true;
            WebViewConfiguration.IOS.UserAgent = "moljac++";

            string dump = WebViewConfiguration.IOS.ToString();
#elif __ANDROID__
            WebViewConfiguration.Android.UserAgent = "moljac++";

            string dump = WebViewConfiguration.Android.UserAgent.ToString();
#elif WINDOWS_UWP
#else
#error Unknown platform
#endif
        }
    }
}
