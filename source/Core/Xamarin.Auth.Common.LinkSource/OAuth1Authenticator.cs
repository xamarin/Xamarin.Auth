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
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Text;

using Xamarin.Utilities;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// Type of method used to fetch the username of an account
    /// after it has been successfully authenticated.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal delegate Task<string> GetUsernameAsyncFunc (IDictionary<string, string> accountProperties);
    #else
    public delegate Task<string> GetUsernameAsyncFunc(IDictionary<string, string> accountProperties);
    #endif

    /// <summary>
    /// OAuth 1.0 authenticator.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal class OAuth1Authenticator 
        : 
        //WebAuthenticator
        WebRedirectAuthenticator        //mc++ changed in 1.5.0.4 on 2017-09-15
                                        // OAuth2Authenticator inherits from WebRedirectAuthenticator
    #else
    public class OAuth1Authenticator
        :
        //WebAuthenticator              
        WebRedirectAuthenticator        //mc++ changed in 1.5.0.4 on 2017-09-15
                                        // OAuth2Authenticator inherits from WebRedirectAuthenticator
    #endif
    {
        string consumerKey;
        string consumerSecret;

        Uri requestTokenUrl;
        Uri authorizeUrl;
        Uri accessTokenUrl;
        Uri callbackUrl;

        GetUsernameAsyncFunc getUsernameAsync;

        string token;
        string tokenSecret;

        string verifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Auth.OAuth1Authenticator"/> class.
        /// </summary>
        /// <param name='consumerKey'>
        /// Consumer key.
        /// </param>
        /// <param name='consumerSecret'>
        /// Consumer secret.
        /// </param>
        /// <param name='requestTokenUrl'>
        /// Request token URL.
        /// </param>
        /// <param name='authorizeUrl'>
        /// Authorize URL.
        /// </param>
        /// <param name='accessTokenUrl'>
        /// Access token URL.
        /// </param>
        /// <param name='callbackUrl'>
        /// Callback URL.
        /// </param>
        /// <param name='getUsernameAsync'>
        /// Method used to fetch the username of an account
        /// after it has been successfully authenticated.
        /// </param>
        public OAuth1Authenticator
                        (
                            string consumerKey, 
                            string consumerSecret,
                            Uri requestTokenUrl, 
                            Uri authorizeUrl, 
                            Uri accessTokenUrl, 
                            Uri callbackUrl,
                            GetUsernameAsyncFunc getUsernameAsync = null,
                            bool isUsingNativeUI = false
                        )
            : base(authorizeUrl, callbackUrl)
        {
            this.is_using_native_ui = isUsingNativeUI;

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentException("consumerKey must be provided", "consumerKey");
            }
            this.consumerKey = consumerKey;

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentException("consumerSecret must be provided", "consumerSecret");
            }
            this.consumerSecret = consumerSecret;

            if (requestTokenUrl == null)
            {
                throw new ArgumentNullException("requestTokenUrl");
            }
            this.requestTokenUrl = requestTokenUrl;

            if (authorizeUrl == null)
            {
                throw new ArgumentNullException("authorizeUrl");
            }
            this.authorizeUrl = authorizeUrl;

            if (accessTokenUrl == null)
            {
                throw new ArgumentNullException("accessTokenUrl");
            }
            this.accessTokenUrl = accessTokenUrl;

            if (callbackUrl == null)
            {
                throw new ArgumentNullException("callbackUrl");
            }
            this.callbackUrl = callbackUrl;

            this.getUsernameAsync = getUsernameAsync;

            this.HttpWebClientFrameworkType = Auth.HttpWebClientFrameworkType.WebRequest;

            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"OAuth1Authenticator ");
            sb.AppendLine($"        IsUsingNativeUI = {IsUsingNativeUI}");
            sb.AppendLine($"        callbackUrl = {callbackUrl}");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            return;
        }

        /// <summary>
        /// Method that returns the initial URL to be displayed in the web browser.
        /// </summary>
        /// <returns>
        /// A task that will return the initial URL.
        /// </returns>
        public override Task<Uri> GetInitialUrlAsync(Dictionary<string, string> query_parameters = null)
        {
            /*
                mc++
                OriginalString property of the Uri object should be used instead of AbsoluteUri

                otherwise trailing slash is added.

                string[] uris = new string[]
                {
                    "http://xamarin.com/",
                    "http://xamarin.com",
                };
                foreach (string u in uris)
                {
                    uri = new Uri(u);
                    Console.WriteLine("uri.AbsoluteUri = " + uri.AbsoluteUri);
                    Console.WriteLine("uri.OriginalString = " + uri.OriginalString);
                }

                The problem is whether to send original string to be compared with registered
                redirect_url on the authorization server od "correct" url (AblsoluteUrl) with
                slash
			*/
            //string oauth_callback_uri_absolute = callbackUrl.AbsoluteUri;
            string oauth_callback_uri_original = callbackUrl.OriginalString;

            //System.Diagnostics.Debug.WriteLine("GetInitialUrlAsync callbackUrl.AbsoluteUri    = " + oauth_callback_uri_absolute);
            System.Diagnostics.Debug.WriteLine("GetInitialUrlAsync callbackUrl.OriginalString = " + oauth_callback_uri_original);

            string oauth_callback_uri = oauth_callback_uri_original;

            var req = OAuth1.CreateRequest
                            (
                                "GET",
                                requestTokenUrl,
                                new Dictionary<string, string>()
                                {
                                    { "oauth_callback", oauth_callback_uri },
                                },
                                consumerKey,
                                consumerSecret,
                                ""
                           );

            if (this.HttpWebClientFrameworkType == HttpWebClientFrameworkType.WebRequest)
            {
                return req.GetResponseAsync()
                          .ContinueWith
                          (
                              respTask =>
                                {

                                    var content = respTask.Result.GetResponseText();

                                    var r = WebEx.FormDecode(content);

                                    token = r["oauth_token"];
                                    tokenSecret = r["oauth_token_secret"];

                                    string paramType = authorizeUrl.AbsoluteUri.IndexOf("?") >= 0 ? "&" : "?";

                                    var url = authorizeUrl.AbsoluteUri + paramType + "oauth_token=" + Uri.EscapeDataString(token);
                                    return new Uri(url);
                                }
                         );
            }
            else if (this.HttpWebClientFrameworkType == HttpWebClientFrameworkType.HttpClient)
            {
                throw new NotImplementedException("HttpClient implementation!");
            }

            return null;
        }

        /// <summary>
        /// Event handler that watches for the callback URL to be loaded.
        /// </summary>
        /// <param name='url'>
        /// The URL of the loaded page.
        /// </param>
        public override void OnPageLoaded(Uri url)
        {
            if (
                    url.Authority == callbackUrl.Authority
                    //url.Host == callbackUrl.Host 
                    &&
                    url.AbsolutePath == callbackUrl.AbsolutePath
                )
            {
                var query = url.Query;
                var r = WebEx.FormDecode(query);

                r.TryGetValue("oauth_verifier", out verifier);

                #if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"OAuth1Authenticator.OnPageLoaded ");
                sb.AppendLine($"        url = {url.AbsoluteUri}");
                sb.AppendLine($"        oauth_verifier = {verifier}");
                System.Diagnostics.Debug.WriteLine(sb.ToString());
                #endif

                GetAccessTokenAsync().ContinueWith(getTokenTask =>
                {
                    if (getTokenTask.IsCanceled)
                    {
                        OnCancelled();
                    }
                    else if (getTokenTask.IsFaulted)
                    {
                        OnError(getTokenTask.Exception);
                    }
                }, TaskContinuationOptions.NotOnRanToCompletion);
            }
            else
            {
                // http[s]://www.xamarin.com != http[s]://xamarin.com
                #if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"OAuth1Authenticator.OnPageLoaded ");
                sb.AppendLine($"        mc++ fix");
                sb.AppendLine($"        url         = {url.AbsoluteUri}");
                sb.AppendLine($"        callbackUrl = {callbackUrl.OriginalString}");
                sb.AppendLine($"        oauth_verifier = {verifier}");
                System.Diagnostics.Debug.WriteLine(sb.ToString());
                #endif
            }

            return;
        }

        Task GetAccessTokenAsync()
        {
            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"OAuth1Authenticator.GetAccessTokenAsync ");
            sb.AppendLine($"        token = {token}");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            RequestParameters = new Dictionary<string, string>
            {
                { "oauth_token", token }
            };

            if (verifier != null)
            {
                RequestParameters["oauth_verifier"] = verifier;
                System.Diagnostics.Debug.WriteLine($"        verifier = {verifier}");
            }

            // WebRequest Replaced with HttpRequest for .net Standard 1.1
            HttpWebRequest req = OAuth1.CreateRequest
                                            (
                                                "GET",
                                                accessTokenUrl,
                                                request_parameters,
                                                consumerKey,
                                                consumerSecret,
                                                tokenSecret
                                            );

            if (this.HttpWebClientFrameworkType == HttpWebClientFrameworkType.WebRequest)
            {
                WebResponse response = req.GetResponseAsync().Result;

                return req.GetResponseAsync().ContinueWith
                            (
                                respTask =>
                                    {
                                        var content = respTask.Result.GetResponseText();

                                        var accountProperties = WebEx.FormDecode(content);

                                        accountProperties["oauth_consumer_key"] = consumerKey;
                                        accountProperties["oauth_consumer_secret"] = consumerSecret;

                                        if (getUsernameAsync != null)
                                        {
                                            getUsernameAsync(accountProperties).ContinueWith(uTask =>
                                            {
                                                if (uTask.IsFaulted)
                                                {
                                                    OnError(uTask.Exception);
                                                }
                                                else
                                                {
                                                    OnSucceeded(uTask.Result, accountProperties);
                                                }
                                            });
                                        }
                                        else
                                        {
                                            OnSucceeded("", accountProperties);
                                        }
                                    }
                            );
            }
            else if (this.HttpWebClientFrameworkType == HttpWebClientFrameworkType.HttpClient)
            {
                throw new NotImplementedException("HttpClient implementation!");
            }

            return null;
        }

        public HttpWebClientFrameworkType HttpWebClientFrameworkType
        {
            get;
            set;
        }

        public override string ToString()
        {
            /*
            string msg = string.Format
                                (
                                    "[OAuth1Authenticator: HttpWebClientFrameworkType={0}]", 
                                   HttpWebClientFrameworkType
                                );
            */
            System.Text.StringBuilder sb = new System.Text.StringBuilder(base.ToString());

            sb.AppendLine().AppendLine(this.GetType().ToString());
            classlevel_depth++;
            string prefix = new string('\t', classlevel_depth);
            sb.Append(prefix).AppendLine($"Query                        = {Query}");
            sb.Append(prefix).AppendLine($"Fragment                     = {Fragment}");
            sb.Append(prefix).AppendLine($"IsLoadableRedirectUri        = {IsLoadableRedirectUri}");
            sb.Append(prefix).AppendLine($"ShouldEncounterOnPageLoading = {ShouldEncounterOnPageLoading}");
            sb.Append(prefix).AppendLine($"ShouldEncounterOnPageLoaded  = {ShouldEncounterOnPageLoaded}");

            return sb.ToString();
        }
    }
}

