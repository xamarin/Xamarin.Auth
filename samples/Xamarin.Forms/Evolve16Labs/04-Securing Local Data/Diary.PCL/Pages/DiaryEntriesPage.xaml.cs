using System;
using Xamarin.Forms;
using Xamarin.Auth;

namespace Diary
{
	public partial class DiaryEntriesPage : ContentPage
	{
		Account account;

		public DiaryEntriesPage (Account account)
		{
			InitializeComponent ();

			this.account = account;

			listEntries.ItemTapped += ListEntries_ItemTapped;
			btnAddDiaryEntry.Clicked += BtnAddDiaryEntryClicked;
		}

		void BtnAddDiaryEntryClicked (object sender, EventArgs e)
		{
			this.Navigation.PushAsync (new EditorPage (account));
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			if(account != null)
				listEntries.ItemsSource = await App.Store.GetEntriesAsync (account.Username);
		}

		async void ListEntries_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			var entry = listEntries.SelectedItem as DiaryEntry;

			await this.Navigation.PushAsync(new EditorPage(account, entry), true);
		}

		async void OnDelete (object sender, EventArgs e)
		{
			var entry = (sender as MenuItem).BindingContext as DiaryEntry;

			await App.Store.DeleteEntryAsync (entry);

			listEntries.ItemsSource = await App.Store.GetEntriesAsync (account.Username);
		}
	}
}