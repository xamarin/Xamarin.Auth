using System;
using System.Diagnostics;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace XamarinAuthBrosten
{
   public partial class MainPage : ContentPage
   {
      public MainPage()
      {
         InitializeComponent();
      }

      private void Button_OnClicked(object sender, EventArgs e)
      {
            var auth = new OAuth2Authenticator(
               clientId: "******",
               clientSecret: "***********",
               scope: "wl.basic",
               authorizeUrl: new Uri("https://login.live.com/oauth20_authorize.srf"),
               redirectUrl: new Uri("https://xamarin.com"),
               accessTokenUrl: new Uri("https://login.live.com/oauth20_token.srf")
            );

            auth.ShowErrors = false;
            auth.AllowCancel = true;
            auth.ClearCookiesBeforeLogin = true;

            auth.Completed += AuthOnCompleted;
            auth.Error += AuthOnError;

            var presenter = new OAuthLoginPresenter();
            presenter.Login(auth);
      }


      private void AuthOnCompleted(object sender, AuthenticatorCompletedEventArgs authenticatorCompletedEventArgs)
      {
         // Correctly called after a successful login. The login-screen gets closed nicely and I am returned to the mainpage. Great!
         Debug.WriteLine("AuthOnCompleted - Authenticated: " + authenticatorCompletedEventArgs.IsAuthenticated);
      }

      private void AuthOnError(object sender, AuthenticatorErrorEventArgs authenticatorErrorEventArgs)
      {
         // Then tapping the "back"-button, this method gets called. Here I would be 
         Debug.WriteLine("AuthOnError - Message: " + authenticatorErrorEventArgs.Message);


         (sender as OAuth2Authenticator).OnCancelled();
      }
   }
}
