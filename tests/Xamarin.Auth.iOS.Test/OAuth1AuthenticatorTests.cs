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
	public class OAuth1AuthenticatorTests
	{
		[Test]
		public void CtorInvalid()
		{
			Assert.That (() => new OAuth1Authenticator (null, "secret", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth1Authenticator ("", "secret", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth1Authenticator ("key", null, new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth1Authenticator ("key", "", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.ArgumentException);
			Assert.That (() => new OAuth1Authenticator ("key", "secret", null, new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That (() => new OAuth1Authenticator ("key", "secret", new Uri ("http://xamarin.com"), null, new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That (() => new OAuth1Authenticator ("key", "secret", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), null, new Uri ("http://xamarin.com")), Throws.InstanceOf<ArgumentNullException>());
			Assert.That (() => new OAuth1Authenticator ("key", "secret", new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), new Uri ("http://xamarin.com"), null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor()
		{
			const string key = "key";
			const string secret = "secret";
			var authUri = new Uri ("http://xamarin.com/auth");
			var requestTokenUri = new Uri ("http://xamarin.com/request");
			var accessTokenUri = new Uri ("http://xamarin.com/access");
			var callbackTokenUri = new Uri ("http://xamarin.com/callback");

			var oauth = new OAuth1Authenticator (key, secret, requestTokenUri, authUri, accessTokenUri, callbackTokenUri);

			Assert.That (oauth.ConsumerKey, Is.EqualTo (key));
			Assert.That (oauth.ConsumerSecret, Is.EqualTo (secret));
			Assert.That (oauth.AuthorizeUrl, Is.EqualTo (authUri));
			Assert.That (oauth.RequestTokenUrl, Is.EqualTo (requestTokenUri));
			Assert.That (oauth.AccessTokenUrl, Is.EqualTo (accessTokenUri));
			Assert.That (oauth.CallbackUrl, Is.EqualTo (callbackTokenUri));
		}
	}
}