using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using XamarinAuth;

[assembly: 
	Xamarin.Forms.ExportRenderer
			(
			// ViewElement to be rendered (from Portable/Shared)
			typeof(HolisticWare.XamarinForms.Authentication.PageLogin),
			// platform specific Renderer : global::Xamarin.Forms.Platform.Android.PageRenderer
			typeof(HolisticWare.XamarinForms.Authentication.XamarinAndroid.PageLoginRenderer)
			)
]

namespace HolisticWare.XamarinForms.Authentication.XamarinAndroid
{
	public partial class PageLoginRenderer : global::Xamarin.Forms.Platform.Android.PageRenderer
	{
		bool done = false;

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (!done) {

				// this is a ViewGroup - so should be able to load an AXML file and FindView<>
				var activity = this.Context as Activity;

				var auth = new global::Xamarin.Auth.OAuth2Authenticator 
					(
					clientId: App.Current.Properties ["clientId"].ToString(),
					scope: App.Current.Properties ["scope"].ToString(),
					authorizeUrl: new Uri( App.Current.Properties ["authorizeUrl"].ToString()),
					redirectUrl: new Uri(App.Current.Properties ["redirectUrl"].ToString())
					);

				auth.Completed +=  auth_Completed;


				//auth.AllowCancel = false;
				activity.StartActivity (auth.GetUI (activity));
				done = true;
			}
		}

		private void auth_Completed (object sender, global::Xamarin.Auth.AuthenticatorCompletedEventArgs e)
		{
			if (eventArgs.IsAuthenticated) 
			{
				//App.Current.MainPage = new ProfilePage();

				//App.Current.Properties ["access_token"] = eventArgs.Account.Properties ["access_token"].ToString();

				//AccountStore.Create (this).Save (eventArgs.Account, "Google");
			} 
			else 
			{
				// Auth failed - The only way to get to this branch on Google is to hit the 'Cancel' button.
				//App.Current.MainPage = new StartPage();
				//App.Current.Properties ["access_token"] = "";
			}

			return;
		}
		public string AccessToken 
		{
			get; 
			set; 
		}
	}
}