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
            // Can you please include this in the official docs?
            // Without this, the app won't redirect after the auth and will just stuck on the Google home page
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            return true;
        }
    }
}
//  _____   _____   ____    _   _    __        __  _   _  __   __   __        __     _      ____      _____   _   _   ___   ____      ____    _   _   ____    ___   _____   ____      _   _   _____   ____    _____   _   ___ 
// |  ___| |  ___| / ___|  | | | |   \ \      / / | | | | \ \ / /   \ \      / /    / \    / ___|    |_   _| | | | | |_ _| / ___|    | __ )  | | | | |  _ \  |_ _| | ____| |  _ \    | | | | | ____| |  _ \  | ____| | | |__ \
// | |_    | |_    \___ \  | | | |    \ \ /\ / /  | |_| |  \ V /     \ \ /\ / /    / _ \   \___ \      | |   | |_| |  | |  \___ \    |  _ \  | | | | | |_) |  | |  |  _|   | | | |   | |_| | |  _|   | |_) | |  _|   | |   / /
// |  _|   |  _|    ___) | |_| |_|     \ V  V /   |  _  |   | |       \ V  V /    / ___ \   ___) |     | |   |  _  |  | |   ___) |   | |_) | | |_| | |  _ <   | |  | |___  | |_| |   |  _  | | |___  |  _ <  | |___  |_|  |_| 
// |_|     |_|     |____/  (_) (_)      \_/\_/    |_| |_|   |_|        \_/\_/    /_/   \_\ |____/      |_|   |_| |_| |___| |____/    |____/   \___/  |_| \_\ |___| |_____| |____/    |_| |_| |_____| |_| \_\ |_____| (_)  (_) 
//
                                                                                                                                                                                                                            
