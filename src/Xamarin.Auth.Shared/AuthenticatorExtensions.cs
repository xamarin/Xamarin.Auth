using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if PLATFORM_IOS
#if __UNIFIED__
using Foundation;
using UIKit;
using AuthenticateUIType = UIKit.UIViewController;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
#endif
#elif PLATFORM_ANDROID
using AuthenticateUIType = Android.Content.Intent;
using UIContext = Android.Content.Context;
#elif WINDOWS_UWP
using AuthenticateUIType = System.Type;

#else
using AuthenticateUIType = System.Object;
#endif

namespace Xamarin.Auth
{
    public static class AuthenticatorExtensions
    {

        /// <summary>
        /// Gets the UI for this authenticator.
        /// </summary>
        /// <returns>
        /// The UI that needs to be presented.
        /// </returns>
#if PLATFORM_IOS
		public static AuthenticateUIType GetUI (this Authenticator authenticator)
		{
            var wa = authenticator as WebAuthenticator;
            if(wa != null)
            {
			    return new UINavigationController (new WebAuthenticatorController (wa));
            }
            
            var fa = authenticator as FormAuthenticator;
            if (fa != null)
            {
			    return new UINavigationController (new FormAuthenticatorController (fa));
            }
        
            throw new NotSupportedException("No UI is defined for this authenticator type");
        }
#elif PLATFORM_ANDROID
        public static AuthenticateUIType GetUI(this Authenticator authenticator, UIContext context)
        {
            var wa = authenticator as WebAuthenticator;
            if(wa != null)
            { 
                var i = new global::Android.Content.Intent(context, typeof(WebAuthenticatorActivity));
                i.PutExtra("ClearCookies", wa.ClearCookiesBeforeLogin);
                var state = new WebAuthenticatorActivity.State
                {
                    Authenticator = wa,
                };
                i.PutExtra("StateKey", WebAuthenticatorActivity.StateRepo.Add(state));
                return i;
            }
            var fa = authenticator as FormAuthenticator;
            if (fa != null)
            {
                var i = new global::Android.Content.Intent(context, typeof(FormAuthenticatorActivity));
                var state = new FormAuthenticatorActivity.State
                {
                    Authenticator = fa,
                };
                i.PutExtra("StateKey", FormAuthenticatorActivity.StateRepo.Add(state));
                return i;
            }

            throw new NotSupportedException("No UI is defined for this authenticator type");
        }
#elif WINDOWS_UWP
        public static AuthenticateUIType GetUI (this Authenticator authenticator)
		{
            var wa = authenticator as WebAuthenticator;
            if(wa != null)
            {
                return typeof(WebAuthenticatorPage);
            }
            
            var fa = authenticator as FormAuthenticator;
            if (fa != null)
            {
                throw new NotSupportedException("FormsAuthenticator is not yet supported on UWP platform");
            }

            throw new NotSupportedException("No UI is defined for this authenticator type");
        }
#endif
    }
}