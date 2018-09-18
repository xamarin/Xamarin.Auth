using AuthenticateUIType = Android.Content.Intent;
using Context = Android.Content.Context;

namespace Xamarin.Auth
{
    public abstract partial class WebAuthenticator
    {
        protected override AuthenticateUIType GetPlatformUI(Context context)
        {
            if (IsUsingNativeUI)
                return GetPlatformUINative(context);

            return GetPlatformUIEmbeddedBrowser(context);
        }

        protected virtual AuthenticateUIType GetPlatformUINative(Context context)
        {
            throw new System.NotImplementedException();

            // TODO: return WebAuthenticatorNativeBrowserActivity.CreateIntent(context, this);
        }

        protected virtual AuthenticateUIType GetPlatformUIEmbeddedBrowser(Context context)
        {
            return WebAuthenticatorActivity.CreateIntent(context, this);
        }

        public static void ClearCookies()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            global::Android.Webkit.CookieSyncManager.CreateInstance(global::Android.App.Application.Context);
#pragma warning restore CS0618 // Type or member is obsolete

            global::Android.Webkit.CookieManager.Instance.RemoveAllCookie();
        }
    }
}
