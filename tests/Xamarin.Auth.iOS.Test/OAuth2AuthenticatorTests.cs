//
//  Copyright 2014, Xamarin Inc.
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

namespace Xamarin.Auth.Test
{
	[TestFixture]
	public class OAuth2AuthenticatorTests
	{
		[Test]
		public void CtorCodeFlow()
		{
			const string clientId = "client id";
			const string clientSecret = "client secret";
			const string scope = "scope";
			Uri authUri = new Uri ("http://xamarin.com/auth");
			Uri redirectUri = new Uri ("http://xamarin.com/redirect");
			Uri accessUri = new Uri ("http://xamarin.com/access");

			var auth = new OAuth2Authenticator (clientId, clientSecret, scope, authUri, redirectUri, accessUri);
			Assert.That (auth.ClientId, Is.EqualTo (clientId));
			Assert.That (auth.ClientSecret, Is.EqualTo (clientSecret));
			Assert.That (auth.Scope, Is.EqualTo (scope));
			Assert.That (auth.AuthorizeUrl, Is.EqualTo (authUri));
			Assert.That (auth.RedirectUrl, Is.EqualTo (redirectUri));
			Assert.That (auth.AccessTokenUrl, Is.EqualTo (accessUri));
		}

		[Test]
		public void CtorTokenFlow()
		{
			const string clientId = "client id";
			const string scope = "scope";
			Uri authUri = new Uri ("http://xamarin.com/auth");
			Uri redirectUri = new Uri ("http://xamarin.com/redirect");

			var auth = new OAuth2Authenticator (clientId,scope, authUri, redirectUri);
			Assert.That (auth.ClientId, Is.EqualTo (clientId));
			Assert.That (auth.ClientSecret, Is.Null);
			Assert.That (auth.Scope, Is.EqualTo (scope));
			Assert.That (auth.AuthorizeUrl, Is.EqualTo (authUri));
			Assert.That (auth.RedirectUrl, Is.EqualTo (redirectUri));
			Assert.That (auth.AccessTokenUrl, Is.Null);
		}

		[Test]
		public void CtorCodeInvalid()
		{
			Assert.That (() => new OAuth2Authenticator (null, "secret", "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth2Authenticator ("", "secret", "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth2Authenticator ("id", null, "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth2Authenticator ("id", "", "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			// Scope is optional
			Assert.That (() => new OAuth2Authenticator ("id", "secret", "scope", null, new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That (() => new OAuth2Authenticator ("id", "secret", "scope", new Uri ("http://xamarin.com"), null, new Uri ("http://xamarin.com")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That (() => new OAuth2Authenticator ("id", "secret", "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void CtorTokenInvalid()
		{
			Assert.That (() => new OAuth2Authenticator (null, "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth2Authenticator ("", "scope", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			// Scope is optional
			Assert.That (() => new OAuth2Authenticator ("id", "scope", null, new Uri ("http://xamarin.com")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That (() => new OAuth2Authenticator ("id", "scope", new Uri ("http://xamarin.com"), null), Throws.InstanceOf<ArgumentNullException>());
		}
	}
}