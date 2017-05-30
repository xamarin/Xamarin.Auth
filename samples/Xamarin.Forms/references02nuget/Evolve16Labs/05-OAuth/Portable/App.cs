using System;
using Xamarin.Forms;

namespace ComicBook
{
	public class App : Application
	{
		public App ()
		{
            MainPage = new NavigationPage(new MainPage ());
		}
	}
}