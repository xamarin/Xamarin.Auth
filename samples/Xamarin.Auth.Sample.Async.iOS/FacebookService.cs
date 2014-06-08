using System;
using System.Json;
using System.Threading.Tasks;
using MonoTouch.Dialog;
using Xamarin.Auth;

namespace Xamarin.Auth_Async_Sample
{
    public class FacebookService
    {
        public async Task<Account> LoginAsync(DialogViewController dialog, bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
                clientId: "<client i>",
                scope: "<scopes>",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://m.facebook.com/connect/login_success.html"))
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

            try
            {
                auth.Completed += d1;
                var vc = auth.GetUI();
                dialog.PresentViewController(vc, true, null);
                var result= await tcs1.Task;
                return result.Account;
            }
            catch (Exception)
            {
               // todo handle the exception
                return null;
            }
            finally
            {
                auth.Completed -= d1;
            }
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