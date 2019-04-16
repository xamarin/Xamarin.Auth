using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;

namespace Xamarin.Auth.XamarinForms
{
    [Preserve(AllMembers = true)]
    #if XAMARIN_AUTH_INTERNAL
    internal class AuthenticatorPage : ContentPage
    #else
    public class AuthenticatorPage : ContentPage
    #endif

    {
        public Authenticator Authenticator
        {
            get;
            set;
        } = null;

        public AuthenticatorPage()
            : base()
        {
            this.Title = "Authenticator Page";

            return;
        }

        public AuthenticatorPage(Authenticator a)
            : this()
        {
            this.Authenticator = a;

            return;
        }

        bool was_shown = false;

        #if XAMARIN_AUTH_INTERNAL
        internal void Authentication_Completed(object sender, AuthenticatorCompletedEventArgs e)
        #else
        public void Authentication_Completed(object sender, AuthenticatorCompletedEventArgs e)
	    #endif
        {
            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IsAuthenticated = {e.IsAuthenticated}");
            sb.AppendLine($"Account.User    = {e.Account?.Username}");
            sb.AppendLine($"access_token    = {e.Account?.Properties["access_token"]}");

            DisplayAlert
                (
                    "Completed",
                    sb.ToString(),
                    "Close"
                );
            #endif

            return;
        }

        #if XAMARIN_AUTH_INTERNAL
        internal void Authentication_Error(object sender, AuthenticatorErrorEventArgs e)
        #else
        public void Authentication_Error(object sender, AuthenticatorErrorEventArgs e)
        #endif
        {
            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Message   = {e.Message}");

			DisplayAlert
				(
                    "Error",
					sb.ToString(),
                    "Close"
                );
            #endif

            return;
        }

        public void Authentication_BrowsingCompleted(object sender, EventArgs e)
        {

            return;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (was_shown)
            {
                Navigation.PopAsync(true);
            }
            else
            {
                was_shown = true;
            }
 
            return;
        }

        protected override void OnDisappearing()
        {
            return;
        }
    }
}

