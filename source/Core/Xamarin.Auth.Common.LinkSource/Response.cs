//
//  Copyright 2012-2016, Xamarin Inc.
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Utilities;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
	/// <summary>
	/// An HTTP web response.
	/// </summary>
	public class Response : IDisposable
	{
		private HttpResponseMessage response;

		/// <summary>
		/// Gets the response URI.
		/// </summary>
		/// <value>The actual Uri of the final request returned.</value>
		/// <remarks>
		/// Some requests may automatically redirecet before turning a final response. This
		/// property will return the final <see cref="Uri"/> that this response is actually from.
		/// </remarks>
		public virtual Uri ResponseUri { get; protected set; }

		/// <summary>
		/// Gets the response status code.
		/// </summary>
		/// <value>The status of this response.</value>
		public virtual HttpStatusCode StatusCode { get; protected set; }

		/// <summary>
		/// Gets the headers returned with this response.
		/// </summary>
		/// <value>A <see cref="Dictionary{TKey,TValue}"/> of header names to values.</value>
		public virtual IDictionary<string, string> Headers { get; protected set; }

		/// <summary>
		/// Initializes a new <see cref="Xamarin.Auth.Response"/> that wraps a <see cref="HttpResponseMessage"/>.
		/// </summary>
		/// <param name="response">
		/// The <see cref="HttpResponseMessage"/> that this response will wrap.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="response"/> is <c>null</c>.</exception>
		public Response (HttpResponseMessage response)
		{
			if (response == null)
				throw new ArgumentNullException ("response");

			this.response = response;

			ResponseUri = response.RequestMessage.RequestUri;
			StatusCode = response.StatusCode;

			Headers = new Dictionary<string, string>();
			foreach (var h in response.Headers) {
				Headers[h.Key] = h.Value.First();
			}
		}

		/// <summary>
		/// Initializes a new blank <see cref="Xamarin.Auth.Response"/>.
		/// </summary>
		protected Response ()
		{
		}

		public virtual string GetResponseText()
		{
			return this.response.Content.ReadAsStringAsync().Result;
		}

		public virtual Task<string> GetResponseTextAsync()
		{
			return this.response.Content.ReadAsStringAsync();
		}

		/// <summary>
		/// Gets the response stream.
		/// </summary>
		/// <returns>
		/// The response stream.
		/// </returns>
		public virtual Task<Stream> GetResponseStreamAsync ()
		{
			return this.response.Content.ReadAsStreamAsync();
		}

        public virtual Stream GetResponseStream()
        {
            return GetResponseStreamAsync().Result;
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
			if (disposing) {
				if (response != null) {
					response.Dispose();
					response = null;
				}
			}
		}
	}
}

