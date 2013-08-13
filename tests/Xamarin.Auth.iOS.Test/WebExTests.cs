using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Xamarin.Utilities;

namespace Xamarin.Auth.Test
{
	[TestFixture]
	public class WebExTests
	{
		[TestCase ("{\"string\": \"value\"}","string", "value")]
		[TestCase ("{\"int\": 5000}", "int", "5000")]
		[TestCase ("{\"bool\": true }", "bool", "true")]
		public void JsonDecode (string json, string arg, string value)
		{
			var dict = WebEx.JsonDecode (json);
			string v;
			Assert.That (dict.TryGetValue (arg, out v), Is.True, "Dictionary did not contain argument '" + arg + "'");
			Assert.That (v, Is.EqualTo (value));
		}
	}
}
