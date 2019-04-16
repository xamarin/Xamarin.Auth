using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Diary
{
	public class App : Application
	{
		public static string DatabaseFolder { get; set; }

		static DiaryEntryStore store;
		public static DiaryEntryStore Store
		{
			get
			{
				return store != null ? store : store = new DiaryEntryStore(DatabaseFolder, "DiaryEntries.db3");
			}
		}

		public App()
		{
			var loginPage = new LoginPage ();
			loginPage.LoginSucceeded += (account) =>  MainPage = new NavigationPage (new DiaryEntriesPage (account));

			MainPage = new NavigationPage (loginPage);
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
