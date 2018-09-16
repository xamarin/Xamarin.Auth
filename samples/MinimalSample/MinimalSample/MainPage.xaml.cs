using System;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace MinimalSample
{
    public partial class MainPage : ContentPage
    {
        private class FacebookAuth
        {
            public const string ClientId = "1889013594699403";
            public const string AuthScope = "basic";
            public static readonly Uri AuthorizationUrl = new Uri("https://www.facebook.com/v3.1/dialog/oauth");
            public static readonly Uri CallbackUrl = new Uri($"fb{ClientId}://authorize");
        }

        public static readonly BindableProperty StatusTextProperty =
            BindableProperty.Create(nameof(StatusText), typeof(string), typeof(MainPage));

        private Account currentAccount;

        public MainPage()
        {
            InitializeComponent();

            StatusText = "You are not logged in.";
            LoginCommand = new Command(OnLogin, () => !IsBusy && currentAccount == null);
            LogoutCommand = new Command(OnLogout, () => !IsBusy && currentAccount != null);

            BindingContext = this;
        }

        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        public Command LoginCommand { get; }

        public Command LogoutCommand { get; }

        private void OnLogin()
        {
            IsBusy = true;

            StatusText = "Starting login...";

            var authenticator = new OAuth2Authenticator(
                FacebookAuth.ClientId,
                FacebookAuth.AuthScope,
                FacebookAuth.AuthorizationUrl,
                FacebookAuth.CallbackUrl);

            var presenter = new OAuthLoginPresenter();
            presenter.Completed += OnAuthCompleted;
            presenter.Login(authenticator);

            LoginCommand.ChangeCanExecute();
            LogoutCommand.ChangeCanExecute();
        }

        private void OnLogout()
        {
            // TODO

            currentAccount = null;
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                currentAccount = e.Account;
                StatusText =
                    $"Login completed successfully!\n" +
                    $"Welcome {e.Account.Username}!";
            }
            else
            {
                currentAccount = null;
                StatusText = "Login failed.";
            }

            IsBusy = false;

            LoginCommand.ChangeCanExecute();
            LogoutCommand.ChangeCanExecute();
        }
    }
}
