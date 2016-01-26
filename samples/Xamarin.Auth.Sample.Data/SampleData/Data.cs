using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Auth.Helpers;

namespace Xamarin.Auth.SampleData
{
	/// <summary>
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
	public partial class Data
	{
		public static Dictionary<string, OAuth> TestCases = null;

		static Data()
		{
			TestCases = new Dictionary<string, OAuth>();

			FacebookOAuth2 facebook = new FacebookOAuth2();
			TestCases.Add(facebook.Description,facebook);

			GoogleOAuth2 google = new GoogleOAuth2();
			TestCases.Add(google.Description,google);

			GithubOAuth2 github = new GithubOAuth2();
			TestCases.Add(github.Description,github);

			InstagramOAuth2 instagram = new InstagramOAuth2();
			TestCases.Add(instagram.Description,instagram);

			LinkedInOAuth1 linkedin1 = new LinkedInOAuth1();
			TestCases.Add(linkedin1.Description,linkedin1);

			LinkedInOAuth2 linkedin2 = new LinkedInOAuth2();
			TestCases.Add(linkedin2.Description,linkedin2);

			MicrosoftLiveOAuth2 microsoft_live = new MicrosoftLiveOAuth2();
			TestCases.Add(microsoft_live.Description,microsoft_live);

			TwitterOAuth1 twitter = new TwitterOAuth1();
			TestCases.Add(twitter.Description,twitter);

			AmazonOAuth2 amazon = new AmazonOAuth2();
			TestCases.Add(amazon.Description,amazon);

			return;
		}
	}
}
