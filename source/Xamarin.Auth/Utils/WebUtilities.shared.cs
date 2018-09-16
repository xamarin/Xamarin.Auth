
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Xamarin.Auth
{
	internal static class WebUtilities
	{
		/// <summary>
		/// Encodes the dictionary to a form representation.
		/// </summary>
		/// <param name="inputs">The dictionary to encode.</param>
		/// <returns>A string in the form encoded format of the dictionary.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="inputs"/> is <c>null</c>.</exception>
		public static string FormEncode (this IDictionary<string, string> inputs)
		{
			if (inputs == null)
				throw new ArgumentNullException ("inputs");

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
			if (unencoded == null)
				throw new ArgumentNullException ("unencoded");

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
	}
}
