using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Auth.XamarinForms
{
    public partial class EmbeddedWebViewConfiguration 
        //: Xamarin.Auth.XamarinForms.IEmbeddedWebViewConfiguration
    {
        public bool IsUsingWKWebView
        {
            get
            {
                return global::Xamarin.Auth.WebViewConfiguration.IOS.IsUsingWKWebView;
            }
            set
            {
                global::Xamarin.Auth.WebViewConfiguration.IOS.IsUsingWKWebView = value;

                return;
            }
        }

        public string UserAgent
        {
            get
            {
                // User-Agent tweaks for Embedded WebViews 
                //  Android     WebView
                //  iOS         UIWebView and WKWebView
                return global::Xamarin.Auth.WebViewConfiguration.IOS.UserAgent;
            }
            set
            {
                // User-Agent tweaks for Embedded WebViews 
                //  Android     WebView
                //  iOS         UIWebView and WKWebView
                global::Xamarin.Auth.WebViewConfiguration.IOS.UserAgent = value;

                return;
            }
        }

    }
}
