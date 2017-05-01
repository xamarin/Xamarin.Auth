using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Auth;

namespace Samples.NativeUI.iOS
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

            //=================================================================
            // Walthrough Step 4.1
            //      Intercepting redirect_url and Loading it

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            System.Uri uri_netfx = new System.Uri(url.AbsoluteString);

            WebRedirectAuthenticator wre = null;
            wre = (WebRedirectAuthenticator)
                        global::Xamarin.Auth.XamarinForms.XamarinIOS.
                               AuthenticatorPageRenderer.Authenticator;

            // load redirect_url Page
            wre?.OnPageLoading(uri_netfx);
			//=================================================================

			return true;
        }
    }
}
