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
                return false;
            }
            set
            {
                throw new NotImplementedException("WKWebView cannot be used on Android");
            }
        }

        public string UserAgent
        {
            get
            {
                throw new NotImplementedException("Bait-n-Switch PCL");
            }
            set
            {
                throw new NotImplementedException("Bait-n-Switch PCL");
            }
        }

    }
}
