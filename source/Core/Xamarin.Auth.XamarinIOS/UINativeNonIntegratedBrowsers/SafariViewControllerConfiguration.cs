using System;

using SafariServices;
using Foundation;

namespace Xamarin.Auth
{
    /// <summary>
    /// Custom tabs configuration needed to pass Custom Tabs data for customization to the 
    /// Activity that launches CustomTabs
    /// 
    /// Too many complex classes to implement java interfaces
    ///     Serializable
    /// or
    ///     Parcellable (Android serialization aimed to increase performance)
    /// </summary>
    public static partial class SafariViewControllerConfiguration
    {
        static SafariViewControllerConfiguration()
        {
            color_xamarin_blue = new CoreGraphics.CGColor(0x34, 0x98, 0xdb);
            SafariViewControllerUnsupportedMessage =
                        "SafariViewController is not supported in iOS < 9"
                        + System.Environment.NewLine +
                        "close CustomTabs by navigating back to the app."
                        ;

            //SFSafariViewController

            return;
        }

        static CoreGraphics.CGColor color_xamarin_blue = null;

        public static void Initialize()
        {

            return;
        }

        public static string SafariViewControllerUnsupportedMessage
        {
        	get;
        	set;
        }

    }
}
