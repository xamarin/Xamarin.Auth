# Getting Started

Xamarin.Auth helps you authenticate users against standard authentication mechanisms (e.g. OAuth), and store user credentials. It's also possible to authenticate against "wilder" services. The library is cross-platform, so once you learn it on iOS, you're all set on Android.




## 1. Create and configure an authenticator

Let's authenticate a user to access Skydrive:

    using Xamarin.Auth;
    ...
	var auth = new OAuth2Authenticator (
		clientId: "Client ID from https://manage.dev.live.com/Applications/Index",
		scope: "wl.basic,wl.skydrive",
		authorizeUrl: new Uri ("https://login.live.com/oauth20_authorize.srf"),
		redirectUrl: new Uri ("https://login.live.com/oauth20_desktop.srf"));

Skydrive uses OAuth 2.0 authentication, so we created an `OAuth2Authenticator`. Authenticators are responible for managing the user interface presented to the user and interacting with services to get them authenticated.

Authenticators take a variety of parameters. In this case, the application's client ID, its authorization scope, and Skydrive's various service locations are required.




## 2. Authenticate the user

While authenticators manage their UIs, it's up to you to actually present them on the screen.

	PresentViewController (auth.GetUI (), true, null);

This is a manual step so that you can present the UI however you want: modally, in navigation controllers, in popovers, wherever.

The `GetUI` function will return `UINavigationControllers` on iOS and `Intents` on Android. If we were on Android, we would write the following code to present the UI:

	StartActivity (auth.GetUI (this));

The `Completed` event is fired when the user successfully authenticates or cancels. You can find out if they successfully authenticated by testing the `IsAuthenticated` property of the event args:

	auth.Completed += (s, e) => {
		DismissViewController (true, null);
		if (e.IsAuthenticated) {
			// Use e.Account to do wonderful things
		} else {
			// They decided not to authenticate
		}
	}

(Since we presented the UI, it's up to us to dimiss it too.)

All the information gathered from a successful authentication is stored in the `Account` property of the `Completed` event args.




## 3. Use the Account data

In the case of OAuth, we are most interested in the `access_token` that results from the authentication. It's available as:

	var accessToken = e.Account.Properties ["acess_token"];

You can now use that token to sign requests.

	var request = WebRequest.Create (
		"http://apis.live.net/v5.0/me/skydrive/shared?access_token=" +
		Uri.EscapeDataString (accessToken));




## 4. Store the account

Xamarin.Auth is able to persistently and securely store `Account` objects so that you don't have to bother the user very often. `AccountStore` objects are used for this storage. On iOS, they use the [Keychain](https://developer.apple.com/library/ios/#documentation/security/Reference/keychainservices/Reference/reference.html). On Android, a [KeyStore](http://developer.android.com/reference/java/security/KeyStore.html) is used.

	AccountStore.Create ().Save (e.Account, "Skydrive");

Accounts are stored and uniquely identified using a key comprised of the account's `Username` and a "Service ID", both of which are strings. The service ID is any string that is used when fetching accounts from the store.

If an account already exists, calling `Save` will overwrite it. This is convenient for services that expire the credentials store in the account object.




## 5. Retrieve stored accounts

You can fetch all the `Account` objects stored for a given service:

	IEnumerable<Account> accounts = AccountStore.Create ().FindAccountsForService ("Skydrive");

It's that easy.




## 6. Make your own authenticator

Xamarin.Auth comes with OAuth 1.0 and OAuth 2.0 authenticators ready to go. For standard username/password scenarios, you can derive a new authenticator from `FormAuthenticator`.

If you want to authenticate against a service not covered by this API, fear not, it's extensible! It's very easy to create your own authenticators. Check out <a href="Details.md">Details</a> for details.


