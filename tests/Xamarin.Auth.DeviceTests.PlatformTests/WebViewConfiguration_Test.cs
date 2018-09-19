using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Xamarin.Auth.DeviceTests
{
    public class WebViewConfiguration_Test
    {
        [Fact]
        public async Task Can_Set_Properties()
        {
            await PlatformUtils.OnMainThread(() =>
            {
#if __IOS__
                WebViewConfiguration.IsUsingWKWebView = true;
                WebViewConfiguration.UserAgent = "moljac++";
#elif __ANDROID__
                WebViewConfiguration.UserAgent = "moljac++";
#elif WINDOWS_UWP
#else
#error Unknown platform
#endif
            });
        }
    }
}
