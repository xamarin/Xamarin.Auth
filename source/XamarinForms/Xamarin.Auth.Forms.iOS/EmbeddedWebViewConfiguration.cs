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
        }

        public string UserAgent
        {
            get
            {
                // User-Agent tweaks for Embedded WebViews 
                //  Android     WebView
                //  iOS         WKWebView
                return global::Xamarin.Auth.WebViewConfiguration.IOS.UserAgent;
            }
            set
            {
                // User-Agent tweaks for Embedded WebViews 
                //  Android     WebView
                //  iOS         WKWebView
                global::Xamarin.Auth.WebViewConfiguration.IOS.UserAgent = value;

                return;
            }
        }

    }
}
