using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples.Helpers
{
    public partial class OAuth1 : OAuth
    {
        public OAuth1()
        {
        }

        /// <summary>
        ///		OAuth1
        ///			Twitter:	Consumer Secret / API Secret
        ///			LinkedIn:	Secret key
        ///			google:		
        ///		
        /// </summary>
        public string OAuth1_SecretKey_ConsumerSecret_APISecret
        {
            get;
            set;
        }

        public Uri OAuth1_UriRequestToken
        {
            get;
            set;
        }

    }
}

