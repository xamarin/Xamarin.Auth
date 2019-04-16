//
//  Copyright 2012-2016, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;

#if ! __UNIFIED__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#else
using Foundation;
using UIKit;
#endif

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Utilities.iOS
#else
namespace Xamarin.Utilities._MobileServices.iOS
#endif
{
    public static class UIViewControllerEx
    {
        public static void ShowError(this UIViewController controller, string title, Exception error, Action continuation = null)
        {
            ShowError(controller, title, error.GetUserMessage(), continuation);
        }

        public static void ShowError(this UIViewController controller, string title, string message, Action continuation = null)
        {
            var mainBundle = NSBundle.MainBundle;

            var alert = new UIAlertView
                            (
                                mainBundle.LocalizedString(title, "Error message title"),
                                mainBundle.LocalizedString(message, "Error"),
                                null,
                                mainBundle.LocalizedString("OK", "Dismiss button title for error message")
                            );

            if (continuation != null)
            {
                alert.Dismissed += delegate
                {
                    continuation();
                };
            }

            alert.Show();
        }
    }
}

