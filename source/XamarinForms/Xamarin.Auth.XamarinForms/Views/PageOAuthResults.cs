using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin.Auth.XamarinForms
{
	/// <summary>
	/// Page login.
	/// </summary>
    public partial class PageOAuthResults : ContentPage
	{

		private void DefineUserInterface ()
		{
			TableView table_view = new TableView
			{
				Intent = TableIntent.Form,
			};

			TableRoot root = new TableRoot ("Xamarin.Auth Authenticated")
			{
			};

			TableSection section = new TableSection ("Account Properties")
			{
			};

			List<TextCell> cells_account_properties = new List<TextCell>();
         

			section.Add(cells_account_properties);
			root.Add(section);
			table_view.Root = root;

			Button buttonOAuthProvider = new Button () 
			{
				Text = "Authenticate"
			};
			StackLayout stack_layout = new StackLayout ();
			stack_layout.Children.Add (buttonOAuthProvider);
			stack_layout.Children.Add (table_view);

			this.Content = stack_layout;

			return;
		}

	}
}


