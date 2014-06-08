using System;
using System.Json;
using System.Threading.Tasks;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Xamarin.Auth;

namespace Xamarin.Auth_Async_Sample
{
    public class FacebookService
    {
        public async Task<AuthenticatorCompletedEventArgs> LoginAsync(DialogViewController dialog, bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
                clientId: "<client id>",
                scope: "basic_info, public_profile, email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"))
            {
                AllowCancel = allowCancel
            };

            // If authorization succeeds or is canceled, .Completed will be fired.
            var tcs1 = new TaskCompletionSource<AuthenticatorCompletedEventArgs>();
            EventHandler<AuthenticatorCompletedEventArgs> d1 =
                (o, e) =>
                {
                    try
                    {
                        tcs1.TrySetResult(e);
                    }
                    catch (Exception ex)
                    {
                        tcs1.TrySetResult(new AuthenticatorCompletedEventArgs(null));
                    }
                };
            auth.Completed += d1;
            UIViewController vc = auth.GetUI();
            dialog.PresentViewController(vc, true, null);
            var result= await tcs1.Task;
            auth.Completed -= d1;
            return result;
        }

        public async Task<string> GetUserInfoAsync(Account account)
        {
            try
            {
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account);
                var userInfoResult = await request.GetResponseAsync();
                var obj = JsonValue.Parse(userInfoResult.GetResponseText());
                if (obj != null)
                {
                    // the best solution is to create an object with all parameters, but it is an example :)
                    return obj["name"];
                }
            }
            catch (Exception ex)
            {
                //Todo
            }
            return null;
        }
    }
}