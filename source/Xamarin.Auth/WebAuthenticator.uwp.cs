using AuthenticateUIType = System.Type;

namespace Xamarin.Auth
{
    public abstract partial class WebAuthenticator
    {
        public static void ClearCookies()
        {
            // there is no way to clear cache for WebView
            // http://blogs.msdn.com/b/wsdevsol/archive/2012/10/18/nine-things-you-need-to-know-about-webview.aspx#AN7
        }

        protected override AuthenticateUIType GetPlatformUI()
        {
            if (IsUsingNativeUI)
                return GetPlatformUINative();

            return GetPlatformUIEmbeddedBrowser();
        }

        protected virtual AuthenticateUIType GetPlatformUINative()
        {
            // TODO: implement UWP's OAuth flow

            return typeof(WebAuthenticatorPage);
        }

        protected virtual AuthenticateUIType GetPlatformUIEmbeddedBrowser()
        {
            return typeof(WebAuthenticatorPage);
        }
    }
}
