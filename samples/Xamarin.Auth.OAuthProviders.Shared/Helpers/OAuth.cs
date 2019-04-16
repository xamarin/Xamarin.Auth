using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth.ProviderSamples.Helpers
{
    /*
			WORKING!!
				Facebook
					OAuth2
						scope = "";				// email, basic, ...
						uri_authorize =  new Uri("https://m.facebook.com/dialog/oauth/");
						uri_callback_redirect = new Uri("http://xamarin.com");

				Twitter
					OAuth1
						  "DynVhdIjJDZSdQMb8KSLTA",
						  "REvU5dCUQI4MvjV6aWwUWVUqwObu3tvePIdQVBhNo",
						  new Uri("https://api.twitter.com/oauth/request_token"),
						  new Uri("https://api.twitter.com/oauth/authorize"),
						  new Uri("https://api.twitter.com/oauth/access_token"),
						  new Uri("http://twitter.com"

			LINKS / REFERENCES:

				https://forums.xamarin.com/discussion/3420/xamarin-auth-with-twitter
				https://forums.xamarin.com/discussion/16100/oauth-twitter-and-xamarin-article-authenticate-with-xamarin-auth
				http://visualstudiomagazine.com/articles/2014/04/01/using-oauth-twitter-and-async-to-display-data.aspx?m=2
				https://forums.xamarin.com/discussion/15869/xamarin-auth-twitter-authentication-process-failing
				https://forums.xamarin.com/discussion/4178/does-twitter-oauth-work-with-xamarin-auth
				http://www.codeproject.com/Tips/852742/Simple-Twitter-client-using-Xamarin-Forms-Xamarin

	*/
    /// <summary>
    /// 
	///		Google:		OAuth2
	///		Facebook:	OAuth2
	///					https://developers.facebook.com/apps/
	///					https://developers.facebook.com/
	///		Twitter:	OAuth1
	///					https://apps.twitter.com/apps
	///					https://dev.twitter.com/
	///					https://dev.twitter.com/oauth
	///		LinkedIn:	OAuth1
	///					https://www.linkedin.com/secure/developer
	///					https://developer.linkedin.com/
	///		Instagram:	OAuth2
	///					https://instagram.com/developer/
	///					http://instagram.com/developer/authentication/
    /// </summary>
	/// <see cref="http://forums.xamarin.com/discussion/36687/how-to-pass-paramaters-to-custom-renderers"/>
	/// <see cref="https://hedgehogjim.wordpress.com/2015/01/29/simplify-using-xamarin-auth-with-async-tasks-a-twitter-example/"/>
	/// <see cref="http://thirteendaysaweek.com/2013/04/25/xamarin-ios-and-authentication-in-windows-azure-mobile-services-part-i-configuration/"/>
	/// <see cref="http://chrisrisner.com/Authentication-with-Windows-Azure-Mobile-Services"/>
	/// <see cref="http://alejandroruizvarela.blogspot.com/2014/03/xamarinauth-custom-accounts.html"/>
	/// <see cref="http://blog.falafel.com/using-xamarin-forms-dependencyservice-and-azure-mobile-services-to-add-authentication-to-cross-platform-apps/"/>
	/// <see cref="https://github.com/jsauve/OAuthTwoDemo.XForms"/>
	///
    public abstract partial class OAuth
    {
        public OAuth()
        {
        }

        public string OrderUI
        {
            get;
            set;
        }

        public string ProviderName
        {
        	get;
        	set;
        }

        public string DisplayName
        {
        	get
        	{
        		Type type = this.GetType();

        		return type.ToString().Replace("Xamarin.Auth.ProviderSamples.", "");
        	}
        }

        public string Description
        {
            get;
            set;
        }

        public bool AllowCancel
        {
            get;
            set;
        }

        /// <summary>
        ///		OAuth = OAuth1 and OAuth2
        /// 		Facebook: 	API ID / Client ID
        ///			Twitter:	Consumer key / API key
        ///			LinkedIn:	API key
        ///			google:		
        ///		
        /// </summary>
        public string OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer
        {
            get;
            set;
        }

        /// <summary>
        /// 	OAuth = OAuth1 and OAuth2
        /// </summary>
        public Uri OAuth_UriAuthorization
        {
            get;
            set;
        }

        /// <summary>
        /// 	OAuth = OAuth1 and OAuth2
        /// </summary>
        public Uri OAuth_UriCallbackAKARedirect
        {
            get;
            set;
        }

        public int OAuth_UriCallbackAKARedirectPort
        {
            get;
            set;
        }

        public string OAuth_UriCallbackAKARedirectPath
        {
            get;
            set;
        }

        public Uri OAuth_UriAccessToken_UriRequestToken
        {
        	get;
        	set;
        }


        public Dictionary<string, string> AccountProperties
        {
            get;
            set;
        }

        public string HowToMarkDown
        {
            get;
            set;
        }

        public string HowToMarkDownPrivateSecret
        {
            get;
            set;
        }

        public delegate Task<string> GetProtectedResourceAsyncFunc(IDictionary<string, string> accountProperties);
    }
}

