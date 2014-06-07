using System;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Xamarin.Auth;

namespace Xamarin.Auth_Async_Sample
{
    public class FacebookService
    {
        public async Task<AuthenticatorCompletedEventArgs> LoginAsync(Activity activity, bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
                clientId: "App ID from https://developers.facebook.com/apps",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"))
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
            var intent = auth.GetUI(activity);
            activity.StartActivity(intent);
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
                    return obj["first_name"];
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