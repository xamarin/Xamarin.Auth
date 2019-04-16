using System;
using System.Collections.Generic;
using System.Linq;

#if ! __CLASSIC__
using Foundation;
using UIKit;
using System.IO;
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
        //=================================================================
        // WalkThrough Step 4
        //      Intercepting/Catching/Detecting [redirect] url change 
        //      App Linking / Deep linking - custom url schemes
        //      
        public override bool OpenUrl
                                (
                                    UIApplication application,
                                    NSUrl url,
                                    string sourceApplication,
                                    NSObject annotation
                                )
        {
#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("OpenURL Called");
            sb.Append("     url         = ").AppendLine(url.AbsoluteUrl.ToString());
            sb.Append("     application = ").AppendLine(sourceApplication);
            sb.Append("     annotation  = ").AppendLine(annotation?.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(url.AbsoluteString);

            // load redirect_url Page
            TestProvidersController.Auth2?.OnPageLoading(uri_netfx);
            TestProvidersController.Auth1?.OnPageLoading(uri_netfx);

            Notify(url);

            return true;
        }

        private void Notify(NSUrl url)
        {
            NSUrlComponents components = new NSUrlComponents(url, false);

            NSUrlQueryItem token_part =
                            (
                                from NSUrlQueryItem qi in components.QueryItems
                                where qi.Name == "token"
                                select qi
                            ).FirstOrDefault();

            if (token_part != null)
            {
                NSNotification notification = new NSNotification(null);

                NSNotificationCenter.DefaultCenter.PostNotification(notification);
            }


        }

    }
}

