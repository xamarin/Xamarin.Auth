using System;
using Xamarin.Auth;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using MonoTouch.Dialog;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif

namespace Xamarin.Auth
{
	public class LoginManager : AlaskaLoginManager
	{
		private static LoginManager instance;
		StringElement loginStatus = new StringElement (String.Empty);
		/// <summary>
		/// Gets or sets the Parent UIViewController.
		/// </summary>
		/// <value>The UIViewController of the caller</value>
		public UIViewController ParentViewController { get; set; }
		//LoadingOverlay loadingOverlay;
		private Account oAccount = null;
		private bool hasCachedAuthToken = false;
		private string ServiceId = "AlaskaLogin";

		public override bool HasCacheAuthToken() {
			hasCachedAuthToken = false;
			IEnumerable<Account> accounts = AccountStore.Create().FindAccountsForService(OAuthSettings.ClientId);
			if (accounts != null) {
				foreach (Account account in accounts) {
					if (!String.IsNullOrWhiteSpace (OAuth2Request.GetAuthorizationHeader (account))) {
						hasCachedAuthToken = true;
						oAccount = account;
					}
					break;
				}
			}
			return hasCachedAuthToken;
		}

		private LoginManager() {
		}

		public static LoginManager Instance
		{
			get {
				if (instance == null) 
					instance = new LoginManager();
				return instance;
			}
		}

		/// <summary>
		/// Logins to Alaska Airlines Federation with OpenID Connect
		/// </summary>
		/// <param name="allowCancel">If set to <c>true</c> allow cancel.</param>
		public async override void LoginToAlaska (bool allowCancel)
		{
			bool isTokenExpired = true;
			try {
				isTokenExpired = oAccount.IsTokenExpired;
				if (!hasCachedAuthToken) {
					AuthenticateUser (allowCancel);
				} else {
					if (isTokenExpired)
						await RefreshToken();
					else
						GetUserInfo (oAccount);
				}
			} catch (Exception) {
				AuthenticateUser (allowCancel);
			}
		}
		
		public override void Logout()
		{
			base.Logout();
			if (HasCacheAuthToken()) {
				AccountStore.Create().Delete(oAccount, ServiceId);
			}
		} 

		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private async Task RefreshToken()
        {
            //loadingOverlay = ProgressDialog.createProgressDialog(mController);
            OAuth2Authenticator auth;
            if (String.IsNullOrWhiteSpace(OAuthSettings.ClientSecret))
                auth = new OAuth2Authenticator(
                    clientId: OAuthSettings.ClientId,
                    scope: OAuthSettings.Scope,
                    authorizeUrl: new Uri(OAuthSettings.AuthorizeUrl),
                    redirectUrl: new Uri(OAuthSettings.RedirectUrl));
            else
                auth = new OAuth2Authenticator(
                    clientId: OAuthSettings.ClientId,
                    clientSecret: OAuthSettings.ClientSecret,
                    scope: OAuthSettings.Scope,
                    authorizeUrl: new Uri(OAuthSettings.AuthorizeUrl),
                    redirectUrl: new Uri(OAuthSettings.RedirectUrl),
                    accessTokenUrl: new Uri(OAuthSettings.AccessTokenUrl));

            auth.ShowUIErrors = false;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, e) =>
            {
                if (!e.IsAuthenticated)
                {
                    loginStatus.Caption = "Not authorized";
                    //ProgressDialog.dismissProgressDialog(loadingOverlay);
                    return;
                }

                GetUserInfo(e.Account);
                AccountStore.Create().Save(e.Account, OAuthSettings.ClientId);
                //ProgressDialog.dismissProgressDialog(loadingOverlay);
            };
			if (oAccount != null)
			{
				var token = oAccount.Properties["refresh_token"];
				try {
					await auth.RequestRefreshTokenAsync(token);
				} catch (Exception) {
					//Xamarin.Insights.Report(ex,"Custom Remark","Probably Failed to refresh token at auth.RequestRefreshTokenAsync", Xamarin.ReportSeverity.Warning);
					AuthenticateUser(false);
				}
			} 
        }

		private void GetUserInfo(Account account)
		{
			Dictionary<string,string> parameters = new Dictionary<string, string>();
			parameters.Add("Authorization",OAuth2Request.GetAuthorizationHeader(account));
			var request = new OAuth2Request ("GET", new Uri (OAuthSettings.UserInfoUrl), parameters , account);

			request.GetResponseAsync().ContinueWith (t => {
				if (t.IsFaulted)
				{	loginStatus.Caption = "Error: " + t.Exception.InnerException.Message;

					AuthenticateUser(true);
				}
				else if (t.IsCanceled)
					loginStatus.Caption = "Canceled";
				else
				{
					var obj = JsonValue.Parse (t.Result.GetResponseText());
					loginStatus.Caption = "Logged in as " + obj["name"];
					Dictionary<string,string> userinfos = new Dictionary<string, string>();
					foreach (string key in userInfoKeys) {
						if (obj.ContainsKey(key)) {
							userinfos.Add(key,obj[key]);
							switch (key) {
							case "email":
								Email = obj[key];
								break;
							case "name":
								FullName = obj[key];
								break;
							case "emplid":
								EmployeeId = obj[key];
								break;
							}
						}
					}
					LoginManager.Instance.OnSuccessfulLogin(userinfos);

				}
			}, uiScheduler);		
		}

		private void AuthenticateUser(bool allowCancel)
		{
			//loadingOverlay = ProgressDialog.createProgressDialog(mController);
			OAuth2Authenticator auth;
			if (String.IsNullOrWhiteSpace(OAuthSettings.ClientSecret))
				auth = new OAuth2Authenticator (
					clientId: OAuthSettings.ClientId,
					scope: OAuthSettings.Scope,
					authorizeUrl: new Uri (OAuthSettings.AuthorizeUrl),
					redirectUrl: new Uri (OAuthSettings.RedirectUrl));
			else
				auth = new OAuth2Authenticator (
					clientId: OAuthSettings.ClientId,
					clientSecret: OAuthSettings.ClientSecret,
					scope: OAuthSettings.Scope,
					authorizeUrl: new Uri (OAuthSettings.AuthorizeUrl),
					redirectUrl: new Uri (OAuthSettings.RedirectUrl),
					accessTokenUrl: new Uri (OAuthSettings.AccessTokenUrl));

			auth.AllowCancel = allowCancel;
			auth.ShowUIErrors = false;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, e) =>
			{
				// We presented the UI, so it's up to us to dismiss it.
				ParentViewController.DismissViewController (true, null);

				if (!e.IsAuthenticated) {
					loginStatus.Caption = "Not authorized";
					//ProgressDialog.dismissProgressDialog(loadingOverlay);
					//mController.ReloadData();
					return;
				}

				GetUserInfo(e.Account);
				AccountStore.Create().Save(e.Account, OAuthSettings.ClientId);
				//((ILoginEvents)mController).OnSuccessfulLoginIn(employeeId);
			};
			UIViewController vc = auth.GetUI();
			//make sure it's a navigation controller then set the BarStyle
			if (vc.GetType() == typeof(UINavigationController))
				((UINavigationController)vc).NavigationBar.BarStyle = UIBarStyle.Black;
			ParentViewController.PresentViewController (vc, true, null);
		}
	}
}