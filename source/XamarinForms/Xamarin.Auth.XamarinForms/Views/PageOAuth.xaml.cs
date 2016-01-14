using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin.Auth.XamarinForms.Views
{
	public partial class PageOAuth : ContentPage
	{
        public PageOAuth ()
		{
			InitializeComponent ();
		}
        
		Page page = null;

		public OAuth1Authenticator OAuth1Authenticator = null;
        public OAuth2Authenticator OAuth2Authenticator = null;

        public void OnItemSelected (object sender, ItemTappedEventArgs args_tapped)
		{
			object item = args_tapped.Item;

			KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;

			string authentication_provider = kvp.Value;

			//switch (authentication_provider)
			//{
			//	default:
			//		string msg = "Unknown Authentication Provider: " + authentication_provider;
			//		//throw new NotImplementedException(msg);
			//}

			this.Navigation.PushAsync(page);

			return;

		}


	}
}

