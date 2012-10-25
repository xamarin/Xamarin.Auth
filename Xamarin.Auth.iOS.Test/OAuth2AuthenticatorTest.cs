//
//  Copyright 2012, Xamarin Inc.
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
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;
using System.Threading.Tasks;

namespace Xamarin.Auth.iOS.Test
{
	[TestFixture]
	public class OAuth2AuthenticatorTest
	{
		void HandleCompleted (object sender, AuthenticatorCompletedEventArgs e)
		{
			AppDelegate.SharedViewController.DismissViewController (true, null);
			if (e.IsAuthenticated) {
				Console.WriteLine ("AUTHENTICATED: " + e.Account.Serialize ());
			}
			else {
				Console.WriteLine ("NOT AUTHENTICATED");
			}
		}

		[Test]
		public void Manual_Token_Facebook ()
		{
			var a = new OAuth2Authenticator (
				clientId: "346691492084618",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += HandleCompleted;
		}

		[Test]
		public void Manual_Code_Bitly ()
		{
			var a = new OAuth2Authenticator (
				clientId: "a939c411b51233c12138c7394c970eb578365602",
				clientSecret: "",
				scope: "",
				authorizeUrl: new Uri ("https://bitly.com/oauth/authorize"),
				redirectUrl: new Uri ("http://xamarin.com/"),
				accessTokenUrl: new Uri ("https://api-ssl.bitly.com/oauth/access_token"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += HandleCompleted;
		}

		[Test]
		public void Manual_Code_Github ()
		{
			var a = new OAuth2Authenticator (
				clientId: "ba91626eac8abeccc336",
				clientSecret: "",
				scope: "user,gist",
				authorizeUrl: new Uri ("https://github.com/login/oauth/authorize"),
				redirectUrl: new Uri ("http://xamarin.com"),
				accessTokenUrl: new Uri ("https://github.com/login/oauth/access_token"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += HandleCompleted;
		}

		[Test]
		public void Manual_Code_LiveConnect ()
		{
			var a = new OAuth2Authenticator (
				clientId: "00000000440DC040",
				clientSecret: "",
				scope: "wl.basic,wl.share,wl.skydrive",
				authorizeUrl: new Uri ("https://login.live.com/oauth20_authorize.srf"),
				redirectUrl: new Uri ("https://xamarin.com"),
				accessTokenUrl: new Uri ("https://login.live.com/oauth20_token.srf"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += HandleCompleted;
		}

		[Test]
		public void Manual_Token_LiveConnect ()
		{
			var a = new OAuth2Authenticator (
				clientId: "00000000440DC040",
				scope: "wl.basic,wl.share,wl.skydrive",
				authorizeUrl: new Uri ("https://login.live.com/oauth20_authorize.srf"),
				redirectUrl: new Uri ("https://login.live.com/oauth20_desktop.srf"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += HandleCompleted;
		}
	}
}

