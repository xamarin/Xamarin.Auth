using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;

namespace Xamarin.Auth.XamarinForms
{
    [Preserve(AllMembers = true)]
    public class AuthenticatorPage : ContentPage
    {
        public Authenticator Authenticator
        {
            get;
            set;
        } = null;

        public AuthenticatorPage()
            : base()
        {
            return;
        }

        bool was_shown = false;

        public void Authentication_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            #if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IsAuthenticated = {e.IsAuthenticated}");
            sb.AppendLine($"Account.User    = {e.Account.Username}");
            sb.AppendLine($"access_token      = {e.Account.Properties["access_token"]}");

            DisplayAlert
                (
                    "Completed",
                    sb.ToString(),
                    "Close"
                );
            #endif

            return;
        }

        public void Authentication_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Message   = {e.Message}");

			DisplayAlert
				(
                    "Error",
					sb.ToString(),
                    "Close"
                );

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
    }
}

