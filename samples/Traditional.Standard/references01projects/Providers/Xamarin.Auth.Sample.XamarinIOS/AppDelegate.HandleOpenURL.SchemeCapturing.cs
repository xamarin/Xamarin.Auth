using System;
using System.Collections.Generic;
using System.Linq;

#if ! __CLASSIC__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace Xamarin.Auth.Sample.XamarinIOS
{
    public partial class AppDelegate
    {
        /// <summary>
        /// Opens the URL.
        /// 
        /// 
        /// </summary>
        /// <returns><c>true</c>, if URL was opened, <c>false</c> otherwise.</returns>
        /// <param name="app">App.</param>
        /// <param name="url">URL.</param>
        /// <param name="sourceApp">Source app.</param>
        /// <param name="annotation">Annotation.</param>
        /// <example>
        /// <code>
        /// <!--
        ///     Info.plist
        /// -->
        ///     <key>CFBundleURLTypes</key>
        ///    <array>
        ///        <dict>
        ///            <key>CFBundleURLName</key>
        ///            <string>com.example.store</string>
        ///            <key>CFBundleURLTypes</key>
        ///            <string>Viewer</string>
        ///            <key>CFBundleURLSchemes</key>
        ///            <array>
        ///                <string>xamarinauth</string>
        ///                <string>xamarin-auth</string>
        ///                <string>xamarin.auth</string>
        ///             </array>
        ///        </dict>
        ///    </array>
        /// </code>
        /// </example>
        public override bool OpenUrl
                                (
                                    UIApplication application,
                                    NSUrl url,
                                    string sourceApplication,
                                    NSObject annotation
                                )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("OpenURL Called");
            sb.Append("     url         = ").AppendLine(url.AbsoluteUrl.ToString());
            sb.Append("     application = ").AppendLine(sourceApplication);
            sb.Append("     annotation  = ").AppendLine(annotation?.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());

            System.Uri uri = new Uri(url.AbsoluteString);
            IDictionary<string, string> fragment = Utilities.WebEx.FormDecode(uri.Fragment);

            Account account = new Account
                                    (
                                        "username",
                                        new Dictionary<string, string>(fragment)
                                    );

            AuthenticatorCompletedEventArgs args_completed = new AuthenticatorCompletedEventArgs(account);

            if (TestProvidersController.Auth2 != null)
            {
                TestProvidersController.Auth2.OnSucceeded(account);
            }
            else if (TestProvidersController.Auth1 != null)
            {
                TestProvidersController.Auth1.OnSucceeded(account);
            }
            else
            {
            }

            return true;
        }

    }
}

