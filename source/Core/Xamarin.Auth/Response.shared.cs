using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public class Response : IDisposable
    {
        private HttpResponseMessage response;

        protected Response()
        {
        }

        public Response(HttpResponseMessage response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));

            ResponseUri = response.RequestMessage.RequestUri;
            StatusCode = response.StatusCode;

            Headers = new Dictionary<string, string>();
            foreach (var h in response.Headers)
            {
                Headers[h.Key] = h.Value?.FirstOrDefault();
            }
        }

        public virtual Uri ResponseUri { get; protected set; }

        public virtual HttpStatusCode StatusCode { get; protected set; }

        public virtual IDictionary<string, string> Headers { get; protected set; }

        public virtual string GetResponseText()
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        public virtual Task<string> GetResponseTextAsync()
        {
            return response.Content.ReadAsStringAsync();
        }

        public virtual Task<Stream> GetResponseStreamAsync()
        {
            return response.Content.ReadAsStreamAsync();
        }

        public virtual Stream GetResponseStream()
        {
            return GetResponseStreamAsync().Result;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", StatusCode, ResponseUri);
        }

        public void Dispose()
        {
            response?.Dispose();
        }
    }
}
