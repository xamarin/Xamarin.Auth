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
using System.Threading.Tasks;
using System.Text;

using Xamarin.Utilities.iOS;
using Xamarin.Controls;

#if !__UNIFIED__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#else
using Foundation;
using UIKit;
using WebKit;
#endif

namespace Xamarin.Auth
{
    internal partial class WebAuthenticatorController
    {
        //==============================================================================================================
        #region     WKWebView
        /// <summary>
        /// WKWebView WKWebViewNavigationDelegate, WKWebViewUIDelegate, WKWebViewJacascriptMessageHandler
        /// </summary>
        internal class WKWebViewNavigationDelegate : WebKit.WKNavigationDelegate
        {
            public override void DecidePolicy
                                    (
                                        WKWebView webView,
                                        WKNavigationAction navigationAction,
                                        Action<WKNavigationActionPolicy> decisionHandler
                                    )
            {
                // Navigation Allowed?

                //decison_handler(WKNavigationActionPolicy.Cancel);    
                //decison_handler(WKNavigationActionPolicy.Allow);

                return;
            }

            public override void DidFailNavigation
                                    (
                                        WKWebView webView,
                                        WKNavigation navigation,
                                        NSError error
                                    )
            {
                // Navigation Failed

                return;
            }

            public override void DidFailProvisionalNavigation
                                    (
                                        WKWebView webView,
                                        WKNavigation navigation,
                                        NSError error
                                    )
            {
                // Provisional Navigation Failed? WAT?

                return;
            }

            public override void DidStartProvisionalNavigation
                                    (
                                        WKWebView webView,
                                        WKNavigation navigation
                                    )
            {
                // Provisional Navigation Started

                return;
            }

            public override void DidFinishNavigation
                                    (
                                        WKWebView webView, 
                                        WKNavigation navigation
                                    )
            {
                // Navigation Finished

                return;
            }

            public override void DidCommitNavigation
                                    (
                                        WKWebView webView,
                                        WKNavigation navigation
                                    )
            {
                // mandatory

                return;
            }
        }

        protected class WKWebViewUIDelegate : WebKit.WKUIDelegate
        {
            public override void RunJavaScriptAlertPanel
                                    (
                                        WKWebView webView, 
                                        string message, 
                                        WKFrameInfo frame, 
                                        Action completionHandler
                                    )
            {
                // custom javascript Alert() code possible

                return;
            }
        }

        protected class WKWebViewJacascriptMessageHandler : WebKit.WKScriptMessageHandler
        {
            public override void DidReceiveScriptMessage
                                    (
                                        WKUserContentController userContentController,
                                        WKScriptMessage message
                                    )
            {
                // do whatever you need to do with the message here

                return;
            }
        }
        #endregion  WKWebView
        //==============================================================================================================
    }
}

