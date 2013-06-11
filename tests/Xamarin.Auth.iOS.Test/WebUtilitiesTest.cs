//
//  Copyright 2013, Xamarin Inc.
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
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Xamarin.Auth.Test
{
	[TestFixture]
	public class WebUtilitiesTest
	{
		[Test]
		public void FormEncodeInvalid()
		{
			Assert.That (() => WebUtilities.EncodeString (null), Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		public void FormEncodeEmpty()
		{
			string result = new Dictionary<string, string>().FormEncode();
			Assert.That (result, Is.EqualTo (String.Empty));
		}

		[Test]
		public void EncodeStringInvalid()
		{
			Assert.That (() => WebUtilities.FormEncode (null), Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		public void EncodeStringEmpty()
		{
			string unencoded = String.Empty;
			string result = WebUtilities.EncodeString (unencoded);
			Assert.That (result, Is.EqualTo (unencoded));
		}
	}
}