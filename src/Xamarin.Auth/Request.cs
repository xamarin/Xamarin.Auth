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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Auth;
using Xamarin.Utilities;

namespace Xamarin.Auth
{
	/// <summary>
	/// An HTTP web request that provides a convenient way to make authenticated
	/// requests using account objects obtained from an authenticator.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal class Request
#else
	public class Request
#endif
	{
		HttpWebRequest request;

		/// <summary>
		/// The HTTP method.
		/// </summary>
		/// <value>A string representing the HTTP method to be used with this request.</value>
		public string Method { get; protected set; }

		/// <summary>
		/// The URL of the resource to request.
		/// </summary>
		public Uri Url { get; protected set; }

		/// <summary>
		/// The parameters of the request. These will be added to the query string of the
		/// URL for GET requests, encoded as form a parameters for POSTs, and added as
		/// multipart values if the request uses <see cref="Multiparts"/>.
		/// </summary>
		public IDictionary<string, string> Parameters { get; protected set; }

		/// <summary>
		/// The account that will be used to authenticate this request.
		/// </summary>
		public virtual Account Account { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.Request"/> class.
		/// </summary>
		/// <param name='method'>
		/// The HTTP method.
		/// </param>
		/// <param name='url'>
		/// The URL.
		/// </param>
		/// <param name='parameters'>
		/// Parameters that will pre-populate the <see cref="Parameters"/> property or null.
		/// </param>
		/// <param name='account'>
		/// The account used to authenticate this request.
		/// </param>
		public Request (string method, Uri url, IDictionary<string, string> parameters = null, Account account = null)
		{
			Method = method;
			Url = url;
			Parameters = parameters == null ? 
				new Dictionary<string, string> () :
				new Dictionary<string, string> (parameters);
			Account = account;
		}

		/// <summary>
		/// A single part of a multipart request.
		/// </summary>
		protected class Part
		{
			/// <summary>
			/// The data.
			/// </summary>
			public Stream Data;

			/// <summary>
			/// The optional textual representation of the <see cref="Data"/>.
			/// </summary>
			public string TextData;

			/// <summary>
			/// The name.
			/// </summary>
			public string Name;

			/// <summary>
			/// The MIME type.
			/// </summary>
			public string MimeType;

			/// <summary>
			/// The filename of this part if it represents a file.
			/// </summary>
			public string Filename;
		}

		/// <summary>
		/// The parts of a multipart request.
		/// </summary>
		protected readonly List<Part> Multiparts = new List<Part> ();

		/// <summary>
		/// Adds a part to the request. Doing so will make this request be sent as multipart/form-data.
		/// </summary>
		/// <param name='name'>
		/// Name of the part.
		/// </param>
		/// <param name='data'>
		/// Text value of the part.
		/// </param>
		public void AddMultipartData (string name, string data)
		{
			Multiparts.Add (new Part {
				TextData = data,
				Data = new MemoryStream (Encoding.UTF8.GetBytes (data)),
				Name = name,
				MimeType = "",
				Filename = "",
			});
		}

		/// <summary>
		/// Adds a part to the request. Doing so will make this request be sent as multipart/form-data.
		/// </summary>
		/// <param name='name'>
		/// Name of the part.
		/// </param>
		/// <param name='data'>
		/// Data used when transmitting this part.
		/// </param>
		/// <param name='mimeType'>
		/// The MIME type of this part.
		/// </param>
		/// <param name='filename'>
		/// The filename of this part if it represents a file.
		/// </param>
		public virtual void AddMultipartData (string name, Stream data, string mimeType = "", string filename = "")
		{
			Multiparts.Add (new Part {
				Data = data,
				Name = name,
				MimeType = mimeType,
				Filename = filename,
			});
		}

		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <returns>
		/// The response.
		/// </returns>
		public virtual Task<Response> GetResponseAsync ()
		{
			return GetResponseAsync (CancellationToken.None);
		}

		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <remarks>
		/// Service implementors should override this method to modify the PreparedWebRequest
		/// to authenticate it.
		/// </remarks>
		/// <param name="cancellationToken"></param>
		/// <returns>
		/// The response.
		/// </returns>
		public virtual Task<Response> GetResponseAsync (CancellationToken cancellationToken)
		{
			var request = GetPreparedWebRequest ();

			//
			// Disable 100-Continue: http://blogs.msdn.com/b/shitals/archive/2008/12/27/9254245.aspx
			//
			if (Method == "POST") {
				ServicePointManager.Expect100Continue = false;
			}

			if (Multiparts.Count > 0) {
				var boundary = "---------------------------" + new Random ().Next ();
				request.ContentType = "multipart/form-data; boundary=" + boundary;

				return Task.Factory
						.FromAsync<Stream> (request.BeginGetRequestStream, request.EndGetRequestStream, null)
						.ContinueWith (reqStreamtask => {
						
					using (reqStreamtask.Result) {
						WriteMultipartFormData (boundary, reqStreamtask.Result);
					}
						
					return Task.Factory
									.FromAsync<WebResponse> (request.BeginGetResponse, request.EndGetResponse, null)
									.ContinueWith (resTask => {
						return new Response ((HttpWebResponse)resTask.Result);
					}, cancellationToken);
				}, cancellationToken).Unwrap();
			} else if (Method == "POST" && Parameters.Count > 0) {
				var body = Parameters.FormEncode ();
				var bodyData = System.Text.Encoding.UTF8.GetBytes (body);
				request.ContentLength = bodyData.Length;
				request.ContentType = "application/x-www-form-urlencoded";

				return Task.Factory
						.FromAsync<Stream> (request.BeginGetRequestStream, request.EndGetRequestStream, null)
						.ContinueWith (reqStreamTask => {

					using (reqStreamTask.Result) {
						reqStreamTask.Result.Write (bodyData, 0, bodyData.Length);
					}
							
					return Task.Factory
								.FromAsync<WebResponse> (request.BeginGetResponse, request.EndGetResponse, null)
									.ContinueWith (resTask => {
						return new Response ((HttpWebResponse)resTask.Result);
					}, cancellationToken);
				}, cancellationToken).Unwrap();
			} else {
				return Task.Factory
						.FromAsync<WebResponse> (request.BeginGetResponse, request.EndGetResponse, null)
						.ContinueWith (resTask => {
					return new Response ((HttpWebResponse)resTask.Result);
				}, cancellationToken);
			}
		}

		void WriteMultipartFormData (string boundary, Stream s)
		{
			var boundaryBytes = Encoding.ASCII.GetBytes ("--" + boundary);

			foreach (var p in Multiparts) {
				s.Write (boundaryBytes, 0, boundaryBytes.Length);
				s.Write (CrLf, 0, CrLf.Length);
				
				//
				// Content-Disposition
				//
				var header = "Content-Disposition: form-data; name=\"" + p.Name + "\"";
				if (!string.IsNullOrEmpty (p.Filename)) {
					header += "; filename=\"" + p.Filename + "\"";
				}
				var headerBytes = Encoding.ASCII.GetBytes (header);
				s.Write (headerBytes, 0, headerBytes.Length);
				s.Write (CrLf, 0, CrLf.Length);
				
				//
				// Content-Type
				//
				if (!string.IsNullOrEmpty (p.MimeType)) {
					header = "Content-Type: " + p.MimeType;
					headerBytes = Encoding.ASCII.GetBytes (header);
					s.Write (headerBytes, 0, headerBytes.Length);
					s.Write (CrLf, 0, CrLf.Length);
				}
				
				//
				// End Header
				//
				s.Write (CrLf, 0, CrLf.Length);
				
				//
				// Data
				//
				p.Data.CopyTo (s);
				s.Write (CrLf, 0, CrLf.Length);
			}
			
			//
			// End
			//
			s.Write (boundaryBytes, 0, boundaryBytes.Length);
			s.Write (DashDash, 0, DashDash.Length);
			s.Write (CrLf, 0, CrLf.Length);
		}

		static readonly byte[] CrLf = new byte[] { (byte)'\r', (byte)'\n' };
		static readonly byte[] DashDash = new byte[] { (byte)'-', (byte)'-' };

		/// <summary>
		/// Gets the prepared URL.
		/// </summary>
		/// <remarks>
		/// Service implementors should override this function and add any needed parameters
		/// from the Account to the URL before it is used to get the response.
		/// </remarks>
		/// <returns>
		/// The prepared URL.
		/// </returns>
		protected virtual Uri GetPreparedUrl ()
		{
			var url = Url.AbsoluteUri;

			if (Parameters.Count > 0 && Method != "POST") {
				var head = Url.AbsoluteUri.Contains ('?') ? "&" : "?";
				foreach (var p in Parameters) {
					url += head;
					url += Uri.EscapeDataString (p.Key);
					url += "=";
					url += Uri.EscapeDataString (p.Value);
					head = "&";
				}
			}

			return new Uri (url);
		}

		/// <summary>
		/// Returns the <see cref="T:System.Net.HttpWebRequest"/> that will be used for this <see cref="T:Xamarin.Auth.Request"/>. All properties
		/// should be set to their correct values before accessing this object.
		/// </summary>
		/// <remarks>
		/// Service implementors should modify the returned request to add whatever
		/// authentication data is needed before getting the response.
		/// </remarks>
		/// <returns>
		/// The prepared HTTP web request.
		/// </returns>
		protected virtual HttpWebRequest GetPreparedWebRequest ()
		{
			if (request == null) {
				request = (HttpWebRequest)WebRequest.Create (GetPreparedUrl ());
				request.Method = Method;
			}

			if (request.CookieContainer == null && Account != null) {
				request.CookieContainer = Account.Cookies;
			}

			return request;
		}
	}
}

