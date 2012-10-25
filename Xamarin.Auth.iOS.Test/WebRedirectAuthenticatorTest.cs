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
	public class WebRedirectAuthenticatorTest
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
		public void Manual_Azure ()
		{
			var a = new WebRedirectAuthenticator (
				initialUrl: new Uri ("https://xamarinauth.azure-mobile.net/login/facebook"),
				redirectUrl: new Uri ("https://xamarinauth.azure-mobile.net/login/done"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += HandleCompleted;
		}
	}
}

