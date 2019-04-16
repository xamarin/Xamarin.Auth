//
//  Copyright 2012-2013, Xamarin Inc.
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
using System.Threading.Tasks;
using System.Threading;

using Windows.UI.Xaml.Controls;

using AuthenticateUIType = System.Type;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
    #if XAMARIN_AUTH_INTERNAL
	internal abstract partial class WebAuthenticator
    #else
	public abstract partial class WebAuthenticator 
    #endif
	{
		/// <summary>
		/// Clears all cookies.
		/// </summary>
		/// <seealso cref="ClearCookiesBeforeLogin"/>
		public async static void ClearCookies()
		{
            //await new WebView().ClearCookiesAsync();
            // there is no way to clear cache for WebView
            //http://blogs.msdn.com/b/wsdevsol/archive/2012/10/18/nine-things-you-need-to-know-about-webview.aspx#AN7


        }

        protected override AuthenticateUIType GetPlatformUI()
		{
			//Random r = new Random();
			//string key;

            //do
            //{
            //	key = "xamarin_auth_" + r.Next();
            //} while (PhoneApplicationService.Current.State.ContainsKey(key));
            //
            //PhoneApplicationService.Current.State[key] = this;


            //System.Reflection.Assembly assembly = typeof(Authenticator).Assembly;
            //string assembly_name = assembly.GetName().Name;
            //return new Uri("/" + assembly_name + ";component/WebAuthenticatorPage.xaml?key=" + key, UriKind.Relative);

            return typeof(WebAuthenticatorPage);
		}
	}
}

