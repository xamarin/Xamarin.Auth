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
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Utilities;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
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
        HttpRequestMessage request;

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
        public Request(string method, Uri url, IDictionary<string, string> parameters = null, Account account = null)
        {
            Method = method;
            Url = url;
            Parameters = parameters == null ?
                new Dictionary<string, string>() :
                new Dictionary<string, string>(parameters);
            Account = account;

            return;
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
        protected readonly List<Part> Multiparts = new List<Part>();

        /// <summary>
        /// Adds a part to the request. Doing so will make this request be sent as multipart/form-data.
        /// </summary>
        /// <param name='name'>
        /// Name of the part.
        /// </param>
        /// <param name='data'>
        /// Text value of the part.
        /// </param>
        public void AddMultipartData(string name, string data)
        {
            Multiparts.Add(new Part
            {
                TextData = data,
                Data = new MemoryStream(Encoding.UTF8.GetBytes(data)),
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
        public virtual void AddMultipartData(string name, Stream data, string mimeType = "", string filename = "")
        {
            Multiparts.Add(new Part
            {
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
        public virtual Task<Response> GetResponseAsync()
        {
            return GetResponseAsync(CancellationToken.None);
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
        public virtual async Task<Response> GetResponseAsync(CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();

            var httpRequest = GetPreparedWebRequest();
            httpRequest.Headers.ExpectContinue = false;

            if (Multiparts.Count > 0)
            {
                var boundary = "---------------------------" + new Random().Next();
                var content = new MultipartFormDataContent(boundary);
                foreach (Part part in Multiparts)
                {
                    var partContent = new StreamContent(part.Data);
                    partContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    partContent.Headers.ContentDisposition.FileName = part.Filename;

                    partContent.Headers.ContentType = new MediaTypeHeaderValue(part.MimeType);

                    content.Add(partContent);
                }

                httpRequest.Content = content;

            }
            else if (Method == "POST" && Parameters.Count > 0)
            {
                httpRequest.Content = new FormUrlEncodedContent(Parameters);
            }

            HttpResponseMessage response = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            return new Response(response);
        }

        /*
		 * mc++
		 * never used
		static readonly byte[] CrLf = new byte[] { (byte)'\r', (byte)'\n' };
		static readonly byte[] DashDash = new byte[] { (byte)'-', (byte)'-' };
		*/

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
        protected virtual Uri GetPreparedUrl()
        {
            var url = Url.AbsoluteUri;

            if (Parameters.Count > 0 && Method != "POST")
            {
                #if !PORTABLE
                var head = Url.AbsoluteUri.Contains('?') ? "&" : "?";
                #else
				var head = Url.AbsoluteUri.Contains("?") ? "&" : "?";
                #endif
                foreach (var p in Parameters)
                {
                    url += head;
                    url += Uri.EscapeDataString(p.Key);
                    url += "=";
                    url += Uri.EscapeDataString(p.Value);
                    head = "&";
                }
            }

            return new Uri(url);
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
        protected virtual HttpRequestMessage GetPreparedWebRequest()
        {
            Uri preparedUrl = null;
            if (request == null)
            {
                preparedUrl = GetPreparedUrl();
                request = new HttpRequestMessage(GetMethod(Method), preparedUrl);
            }

            if (Account != null && !request.Headers.Contains("Cookie"))
            {
                if (preparedUrl == null)
                    preparedUrl = GetPreparedUrl();

                CookieCollection cookies = Account.Cookies.GetCookies(preparedUrl);
                if (cookies.Count > 0)
                    request.Headers.Add("Cookie", Account.Cookies.GetCookieHeader(preparedUrl));
            }

            return request;
        }

        static HttpMethod GetMethod(string method)
        {
            method = method.ToUpper();
            switch (method)
            {
                case "GET":
                    return HttpMethod.Get;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "HEAD":
                    return HttpMethod.Head;
                case "DELETE":
                    return HttpMethod.Delete;
                case "TRACE":
                    return HttpMethod.Trace;
                case "OPTIONS":
                    return HttpMethod.Options;
                case "PATCH":
                    return new HttpMethod("PATCH");
                default:
                    throw new ArgumentException("method");
            }
        }
    }
}

