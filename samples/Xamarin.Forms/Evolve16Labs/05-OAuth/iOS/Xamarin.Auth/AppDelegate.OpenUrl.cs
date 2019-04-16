using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using ComicBook;

namespace ComicBook.iOS
{
    public partial class AppDelegate
    {
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
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            return true;
        }
    }
}

