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
using MonoTouch.UIKit;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;

namespace Xamarin.Auth.iOS.Test
{
	[TestFixture]
	public class OAuth2AuthenticatorTest
	{
		[Test]
		public void Manual_CodeGithub ()
		{
			var a = new OAuth2Authenticator (
				clientId: "",
				scope: "",
				authorizeUrl: new Uri ("https://github.com/login/oauth/authorize"),
				redirectUrl: new Uri ("http://xamarin.com"));

			var vc = a.GetUI ();
			AppDelegate.SharedViewController.PresentViewController (vc, true, null);
			a.Completed += (sender, e) => {
				Console.WriteLine (e);
			};
		}
	}
}

