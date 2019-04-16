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

		protected static Xamarin.Auth.Helpers.OAuth oauth = null;
		protected static Xamarin.Auth.Helpers.OAuth1 oauth1 = null;
		protected static Xamarin.Auth.Helpers.OAuth2 oauth2 = null;

		//---------------------------------------------------------------------------------
		// methods implemented in other file which is included in project through wildcard
		// in this case files are in folder
		//			SecureSensitivePrivateData/
		//	patterns
		//			*.secure.cs
		//			*.sensitive.cs
		//			*.sensitive.cs
		//
		//		<Compile Include="SecureSensitivePrivateData/*.secure.cs" />
		//		<Compile Include="SecureSensitivePrivateData/*.secret.cs" />
		//		<Compile Include="SecureSensitivePrivateData/*.sensitive.cs" />
		//
		// csproj include

		// samples for docs
		static partial void SetPublicDemoDataGoogleOAuth2();
		static partial void SetPublicDemoDataFacebookOAuth2();
		static partial void SetPublicDemoDataGithubOAuth2();
		static partial void SetPublicDemoDataTwitterOAuth1();
		static partial void SetPublicDemoDataMicrosoftLiveOAuth2();
		static partial void SetPublicDemoDataInstagramOAuth2();
		static partial void SetPublicDemoDataLinkedInOAuth1();
		static partial void SetPublicDemoDataLinkedInOAuth2();

		// real data (presonal, hidden, secret, sensitive)
		// in csproj included as wildcard:
		//			    <Compile Include="Data\SecurePrivateSecretHidden\*.cs" />
		// data set in PublicDemo is overriden in those methods
		static partial void SetSensitiveDataGoogleOAuth2();
		static partial void SetSensitiveDataFacebookOAuth2();
		static partial void SetSensitiveDataGithubOAuth2();
		static partial void SetSensitiveDataTwitterOAuth1();
		static partial void SetSensitiveDataMicrosoftLiveOAuth2();
		static partial void SetSensitiveDataInstagramOAuth2();
		static partial void SetSensitiveDataLinkedInOAuth1();
		static partial void SetSensitiveDataLinkedInOAuth2();
		//---------------------------------------------------------------------------------

		static Data()
		{
			TestCases = new Dictionary<string, OAuth>();

			SetPublicDemoDataGoogleOAuth2();
			SetPublicDemoDataFacebookOAuth2();
			SetPublicDemoDataGithubOAuth2();
			SetPublicDemoDataTwitterOAuth1();
			SetPublicDemoDataMicrosoftLiveOAuth2();
			SetPublicDemoDataInstagramOAuth2();
			SetPublicDemoDataLinkedInOAuth1();
			SetPublicDemoDataLinkedInOAuth2();

			SetSensitiveDataGoogleOAuth2();
			SetSensitiveDataFacebookOAuth2();
			SetSensitiveDataGithubOAuth2();
			SetSensitiveDataTwitterOAuth1();
			SetSensitiveDataMicrosoftLiveOAuth2();
			SetSensitiveDataInstagramOAuth2();
			SetSensitiveDataLinkedInOAuth1();
			SetSensitiveDataLinkedInOAuth2();

			return;
		}
	}
}
