using LoginAccounts;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace MinimalSample
{
    public partial class MainPage : ContentPage
    {
        public static readonly BindableProperty StatusTextProperty =
            BindableProperty.Create(nameof(StatusText), typeof(string), typeof(MainPage));

        private readonly OAuth2Provider provider = new FacebookHttps();

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
                provider.ClientId,
                provider.Scope,
                provider.AuthorizationUri,
                provider.RedirectUri);

            var presenter = new OAuthLoginPresenter();
            presenter.Completed += OnAuthCompleted;
            presenter.Login(authenticator);

            LoginCommand.ChangeCanExecute();
            LogoutCommand.ChangeCanExecute();

            async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
            {
                if (e.IsAuthenticated)
                {
                    currentAccount = e.Account;

                    StatusText = "Logging you in...";
                    e.Account.Username = await provider.RetriveUsernameAsync(e.Account);
                    StatusText = $"Welcome {e.Account.Username}!";
                }
                else
                {
                    currentAccount = null;

                    StatusText = authenticator.HasCompleted ? "Login cancelled." : "Login failed.";
                }

                IsBusy = false;

                LoginCommand.ChangeCanExecute();
                LogoutCommand.ChangeCanExecute();
            }
        }

        private void OnLogout()
        {
            // TODO

            currentAccount = null;
            StatusText = "Logged out.";
        }
    }
}
