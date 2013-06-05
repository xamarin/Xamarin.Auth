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
using System.Json;
using System.Linq;
using System.Globalization;

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

		/// <summary>
		/// Encodes a string according to: http://www.ietf.org/rfc/rfc3986.txt
		/// </summary>
		/// <returns>
		/// The encoded string.
		/// </returns>
		/// <param name='unencoded'>
		/// The string to encode.
		/// </param>
		public static string EncodeString (string unencoded)
		{
			var utf8 = Encoding.UTF8.GetBytes (unencoded);
			var sb = new StringBuilder ();

			for (var i = 0; i < utf8.Length; i++) {
				var v = utf8[i];
				if ((0x41 <= v && v <= 0x5A) || (0x61 <= v && v <= 0x7A) || (0x30 <= v && v <= 0x39) ||
				    v == 0x2D || v == 0x2E || v == 0x5F || v == 0x7E) {
					sb.Append ((char)v);
				} else {
					sb.AppendFormat (CultureInfo.InvariantCulture, "%{0:X2}", v);
				}
			}

			return sb.ToString ();
		}

		public static string FormEncode (this IDictionary<string, string> inputs)
		{
			var sb = new StringBuilder ();
			var head = "";
			foreach (var p in inputs) {
				sb.Append (head);
				sb.Append (EncodeString (p.Key));
				sb.Append ("=");
				sb.Append (EncodeString (p.Value));
				head = "&";
			}
			return sb.ToString ();
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

		public static Dictionary<string, string> JsonDecode (string encoded)
		{
			var inputs = new Dictionary<string, string> ();
			var json = JsonValue.Parse (encoded) as JsonObject;

			foreach (var kv in json) {
				var v = kv.Value as JsonValue;
				if (v != null) {
					switch (v.JsonType) {
					case JsonType.String:
						inputs [kv.Key] = (string) v;
						break;
					case JsonType.Number:
						inputs [kv.Key] = ((int) v).ToString ();
						break;
					}
				}
			}

			return inputs;
		}
	}
}

