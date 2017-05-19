using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Samples.NativeUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // *
            Xamarin.Auth.OAuth2Authenticator authenticator =
                new Xamarin.Auth.OAuth2Authenticator
                (
                    /*       
                    clientId: "185391188679-9pa23l08ein4m4nmqccr9jm01udf3oup.apps.googleusercontent.com",
                    scope: "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.login",
                    authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                    redirectUrl: new Uri
                                     (
                                         "comauthenticationapp://localhost"
                                     //"com.authentication.app://localhost"
                                     //"com-authentication-app://localhost"
                                     ),
                    */
                           clientId: 
                                new Func<string>
                           (
                                () => 
                                {
                                    // some people are sending the same AppID for google and other providers
                                    // not sure, but google (and others) might check AppID for Native/Installed apps
                                    // Android and iOS against UserAgent in request from 
                                    // CustomTabs and SFSafariViewContorller
                                    // TODO: send deliberately wrong AppID and note behaviour for the future
                                    // fitbit does not care - server side setup is quite liberal
                                    switch (Xamarin.Forms.Device.RuntimePlatform) 
                                    {
                                        case "iOS": 
                                            return "228CVW"; 
                                            break;
                                        case "Android": 
                                            return "228CVW"; 
                                        break;
                                    }
                                    return "oops something is wrong!";
                                }
                          ).Invoke(), 
                    authorizeUrl: new Uri("https://www.fitbit.com/oauth2/authorize"),
                    redirectUrl: new Uri("xamarin-auth://localhost"),
                    scope: "profile",
                    isUsingNativeUI: false
                )
                {
                    AllowCancel = true,
                };

            //---------------------------------------------------------------------
            // ContentPage with CustomRenderers
            Navigation.PushAsync
                    (
                        new Xamarin.Auth.XamarinForms.AuthenticatorPage()
                        {
                            Authenticator  = authenticator,
                        }
                    );
            //---------------------------------------------------------------------
            // Xamarin.UNiversity Team Presenters Concept
            //Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
            //presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            //presenter.Login (authenticator);
            //---------------------------------------------------------------------
            // * /
            return;
        }

        public void Authentication_Completed(object sender, Xamarin.Auth.AuthenticatorCompletedEventArgs e)
        {
        	return;
        }

        public void Authentication_Error(object sender, Xamarin.Auth.AuthenticatorErrorEventArgs e)
        {
        	return;
        }

        public void Authentication_BrowsingCompleted(object sender, EventArgs e)
        {
        	return;
        }
    
    }
}
