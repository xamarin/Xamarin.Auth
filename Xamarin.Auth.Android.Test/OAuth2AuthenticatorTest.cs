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
using Android.NUnit;

namespace Xamarin.Auth.Android.Test
{
	[TestFixture]
	public class OAuth2AuthenticatorTest
	{
		void HandleCompleted (object sender, AuthenticatorCompletedEventArgs e)
		{
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

			var intent = a.GetUI (TestRunner.Shared);
			TestRunner.Shared.StartActivity (intent);
			a.Completed += HandleCompleted;
		}
	}
}

