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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Utilities;

namespace Xamarin.Auth
{
	/// <summary>
	/// An HTTP web response.
	/// </summary>
	public class Response : IDisposable
	{
		HttpWebResponse response;

		/// <summary>
		/// Gets the response URI.
		/// </summary>
		public virtual Uri ResponseUri { get; protected set; }

		/// <summary>
		/// Gets the response status code.
		/// </summary>
		public virtual HttpStatusCode StatusCode { get; protected set; }

		/// <summary>
		/// Gets the headers returned with this response.
		/// </summary>
		public virtual IDictionary<string, string> Headers { get; protected set; }

		/// <summary>
		/// Initializes a new <see cref="Xamarin.Auth.Response"/> that wraps a <see cref="T:System.Net.HttpWebResponse"/>.
		/// </summary>
		/// <param name='response'>
		/// The <see cref="T:System.Net.HttpWebResponse"/> that this response will wrap.
		/// </param>
		public Response (HttpWebResponse response)
		{
			if (response == null) {
				throw new ArgumentNullException ("response");
			}

			this.response = response;

			ResponseUri = response.ResponseUri;
			StatusCode = response.StatusCode;

			Headers = new Dictionary<string, string> ();
			foreach (string h in response.Headers) {
				Headers [h] = response.Headers [h];
			}
		}

		/// <summary>
		/// Initializes a new blank <see cref="Xamarin.Auth.Response"/>.
		/// </summary>
		protected Response ()
		{
		}

		/// <summary>
		/// Reads all the response data and interprets it as a string.
		/// </summary>
		/// <returns>
		/// The response text.
		/// </returns>
		public virtual string GetResponseText ()
		{
			var encoding = Encoding.UTF8;

			if (Headers.ContainsKey ("Content-Type")) {
				encoding = WebEx.GetEncodingFromContentType (Headers ["Content-Type"]);
			}

			using (var s = GetResponseStream ()) {
				using (var r = new StreamReader (s, encoding)) {
					return r.ReadToEnd ();
				}
			}
		}

		/// <summary>
		/// Gets the response stream.
		/// </summary>
		/// <returns>
		/// The response stream.
		/// </returns>
		public virtual Stream GetResponseStream ()
		{
			return response.GetResponseStream ();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Xamarin.Auth.Response"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Xamarin.Auth.Response"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("{0} {1}", StatusCode, ResponseUri);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="Xamarin.Auth.Response"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~Response ()
		{
			Dispose (false);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Xamarin.Auth.Response"/> object.
		/// </summary>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Xamarin.Auth.Response"/> object.
		/// </summary>
		/// <param name='disposing'>
		/// Whether this function was called from the Dispose method.
		/// </param>
		protected virtual void Dispose (bool disposing)
		{
			if (response != null) {
				response.Close ();
				response = null;
			}
		}
	}
}

