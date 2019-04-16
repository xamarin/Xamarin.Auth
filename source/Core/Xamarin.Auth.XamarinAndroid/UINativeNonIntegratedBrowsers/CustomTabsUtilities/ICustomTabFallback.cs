using System;

using Android.App;

#if ! AZURE_MOBILE_SERVICES
namespace Android.Support.CustomTabs.Chromium.SharedUtilities
#else
namespace Android.Support.CustomTabs.Chromium.SharedUtilities._MobileServices
#endif
{
    /// <summary>
    /// To be used as a fallback to open the Uri when Custom Tabs is not available.
    /// </summary>
    #if XAMARIN_CUSTOM_TABS_INTERNAL
    internal interface ICustomTabFallback
    #else
    public interface ICustomTabFallback
    #endif
    {
        /// 
        /// <param name="activity"> The Activity that wants to open the Uri. </param>
        /// <param name="uri"> The uri to be opened by the fallback. </param>
        void OpenUri(Activity activity, Android.Net.Uri uri);
    }
}
