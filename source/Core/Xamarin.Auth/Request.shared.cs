using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public class Request
    {
        private HttpRequestMessage request;
        protected readonly List<Part> Multiparts = new List<Part>();

        public Request(string method, Uri url, IDictionary<string, string> parameters = null, Account account = null)
        {
            Method = method;
            Url = url;
            Parameters = parameters == null ? new Dictionary<string, string>() : new Dictionary<string, string>(parameters);
            Account = account;
        }

        public string Method { get; }

        public Uri Url { get; }

        public IDictionary<string, string> Parameters { get; }

        public Account Account { get; }

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

        public virtual Task<Response> GetResponseAsync()
        {
            return GetResponseAsync(default);
        }

        public virtual async Task<Response> GetResponseAsync(CancellationToken cancellationToken = default)
        {
            using (var client = new HttpClient())
            using (var httpRequest = GetPreparedWebRequest())
            {
                httpRequest.Headers.ExpectContinue = false;

                if (Multiparts.Count > 0)
                {
                    var content = new MultipartFormDataContent(Guid.NewGuid().ToString("N"));
                    foreach (var part in Multiparts)
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

                var response = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                return new Response(response);
            }
        }

        protected virtual Uri GetPreparedUrl()
        {
            var url = Url.AbsoluteUri;

            if (Parameters.Count > 0 && Method != "POST")
            {
                var head = Url.AbsoluteUri.Contains("?") ? "&" : "?";
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

        protected virtual HttpRequestMessage GetPreparedWebRequest()
        {
            var preparedUrl = GetPreparedUrl();

            if (request == null)
                request = new HttpRequestMessage(GetMethod(Method), preparedUrl);

            if (Account != null && !request.Headers.Contains("Cookie"))
            {
                var cookies = Account.Cookies.GetCookies(preparedUrl);
                if (cookies.Count > 0)
                    request.Headers.Add("Cookie", Account.Cookies.GetCookieHeader(preparedUrl));
            }

            return request;
        }

        private static HttpMethod GetMethod(string method)
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
                    throw new ArgumentOutOfRangeException(nameof(method));
            }
        }

        protected class Part
        {
            public Stream Data;

            public string TextData;

            public string Name;

            public string MimeType;

            public string Filename;
        }
    }
}
