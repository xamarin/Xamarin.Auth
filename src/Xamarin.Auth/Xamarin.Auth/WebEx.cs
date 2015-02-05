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
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Json;

namespace Xamarin.Utilities
{
	internal static class WebEx
	{
		public static string GetCookie (this CookieContainer containers, Uri domain, string name)
		{
			var c = containers
					.GetCookies (domain)
					.Cast<Cookie> ()
					.FirstOrDefault (x => x.Name == name);
			return c != null ? c.Value : "";
		}

		public static Encoding GetEncodingFromContentType (string contentType)
		{
			//
			// TODO: Parse the Content-Type
			//
			return Encoding.UTF8;
		}

		public static string GetResponseText (this WebResponse response)
		{
			var httpResponse = response as HttpWebResponse;
			
			var encoding = Encoding.UTF8;
			
			if (httpResponse != null) {
				encoding = GetEncodingFromContentType (response.ContentType);
			}
			
			using (var s = response.GetResponseStream ()) {
				using (var r = new StreamReader (s, encoding)) {
					return r.ReadToEnd ();
				}
			}
		}

		public static Task<WebResponse> GetResponseAsync (this WebRequest request)
		{
			return Task
				.Factory
				.FromAsync<WebResponse> (request.BeginGetResponse, request.EndGetResponse, null);
		}

		static char[] AmpersandChars = new char[] { '&' };
		static char[] EqualsChars = new char[] { '=' };

		public static IDictionary<string, string> FormDecode (string encodedString)
		{
			var inputs = new Dictionary<string, string> ();

			if (encodedString.StartsWith ("?") || encodedString.StartsWith ("#")) {
				encodedString = encodedString.Substring (1);
			}

			var parts = encodedString.Split (AmpersandChars);
			foreach (var p in parts) {
				var kv = p.Split (EqualsChars);
				var k = Uri.UnescapeDataString (kv[0]);
				var v = kv.Length > 1 ? Uri.UnescapeDataString (kv[1]) : "";
				inputs[k] = v;
			}

			return inputs;
		}

		public static Dictionary<string, string> JsonDecode (string encodedString)
		{
			var inputs = new Dictionary<string, string> ();
			var json = JsonValue.Parse (encodedString) as JsonObject;

			foreach (var kv in json) {
				var v = kv.Value as JsonValue;
				if (v != null) {
					if (v.JsonType != JsonType.String)
						inputs[kv.Key] = v.ToString();
					else
						inputs[kv.Key] = (string)v;
				}
			}

			return inputs;
		}

		public static string HtmlEncode (string text)
		{
			if (string.IsNullOrEmpty (text)) {
				return "";
			}

			var sb = new StringBuilder(text.Length);

			int len = text.Length;
			for (int i = 0; i < len; i++) {
				switch (text[i]) {
				case '<':
					sb.Append("&lt;");
					break;
				case '>':
					sb.Append("&gt;");
					break;
				case '"':
					sb.Append("&quot;");
					break;
				case '&':
					sb.Append("&amp;");
					break;
				default:
					if (text[i] > 159) {
						sb.Append ("&#");
						sb.Append (((int)text[i]).ToString (CultureInfo.InvariantCulture));
						sb.Append (";");
					}
					else {
						sb.Append(text[i]);
					}
					break;
				}
			}

			return sb.ToString();
		}

		public static string GetValueFromJson (string json, string key)
		{
		    var p = json.IndexOf ("\"" + key + "\"");
		    if (p < 0) return "";
		    var c = json.IndexOf (":", p);
		    if (c < 0) return "";
		    var q = json.IndexOf ("\"", c);
		    if (q < 0) return "";
		    var b = q + 1;
		    var e = b;
		    for (; e < json.Length && json[e] != '\"'; e++) {
		    }
		    var r = json.Substring (b, e - b);
		    return r;
		}
	}
}

