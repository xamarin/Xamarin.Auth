using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Xaml.Controls;
using AuthenticateUIType = System.Type;

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
	public abstract partial class WebAuthenticator 
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

            // Warning CS1998  This async method lacks 'await' operators and will run 
            // synchronously.Consider using the 'await' operator to await non - blocking API calls, or 
            // 'await Task.Run(...)' to do CPU - bound work on a background thread.	
            System.Threading.Tasks.Task t = null;
            t = System.Threading.Tasks.Task.Run
                                        (
                                            () =>
                                            {
                                            }
                                        );

            return;
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

