using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using LoginAccounts;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace NativeSampleAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private CoordinatorLayout coordinator;
        private ProvidersAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            OAuthLoginPresenter.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            coordinator = FindViewById<CoordinatorLayout>(Resource.Id.coordinator);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var providersList = FindViewById<RecyclerView>(Resource.Id.providersList);
            providersList.SetLayoutManager(new LinearLayoutManager(this));

            adapter = new ProvidersAdapter();
            adapter.ItemClicked += OnItemClicked;
            providersList.SetAdapter(adapter);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void OnItemClicked(ProvidersAdapter.ProviderItem item)
        {
            if (!item.IsAuthenticated)
                Login(item);
            else
                Logout(item);
        }

        private void Login(ProvidersAdapter.ProviderItem item)
        {
            // create authenticator
            Authenticator authenticator = null;
            switch (item.Provider)
            {
                case OAuth1Provider oauth1:
                    break;
                case OAuth2Provider oauth2:
                    authenticator = new OAuth2Authenticator(
                        oauth2.ClientId,
                        oauth2.Scope,
                        oauth2.AuthorizationUri,
                        oauth2.RedirectUri);
                    break;
            }

            // login
            var presenter = new OAuthLoginPresenter();
            presenter.Completed += OnAuthCompleted;
            presenter.Login(authenticator);

            // login complete
            async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
            {
                // update the list item
                item.IsAuthenticated = e.IsAuthenticated;
                adapter.NotifyItemChanged(item);

                if (e.IsAuthenticated)
                {
                    // show some progress
                    var dialog = ShowProgressDialog("Logging you in...");

                    // access the user profile
                    var username = await item.Provider.RetriveUsernameAsync(e.Account);

                    // close the progress and show welcome
                    dialog.Dismiss();
                    //Snackbar.Make(coordinator, $"Welcome {username}!", Snackbar.LengthShort).Show();
                    Toast.MakeText(this, $"Welcome {username}!", ToastLength.Short).Show();
                }
                else
                {
                    new AlertDialog.Builder(this)
                        .SetTitle("Login failed")
                        .SetMessage("There was an issue with the login.")
                        .Show();
                }
            }
        }

        private void Logout(ProvidersAdapter.ProviderItem item)
        {
            item.IsAuthenticated = false;
            adapter.NotifyItemChanged(item);
        }

        private AlertDialog ShowProgressDialog(string title)
        {
            // load the padding attribute value
            int pad = 0;
            using (var array = Theme.ObtainStyledAttributes(new[] { Resource.Attribute.dialogPreferredPadding }))
            {
                pad = (int)array.GetDimension(array.GetIndex(0), 0);
                array.Recycle();
            }

            // create a progress dialog layout
            var progress = new RelativeLayout(this);
            var param = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.WrapContent,
                RelativeLayout.LayoutParams.WrapContent);
            param.SetMargins(pad, pad, pad, pad);
            param.AddRule(LayoutRules.CenterInParent);
            progress.AddView(new ProgressBar(this), param);

            // show the dialog
            return new AlertDialog.Builder(this)
                 .SetView(progress)
                 .SetCancelable(false)
                 .SetTitle(title)
                 .Show();
        }
    }
}
