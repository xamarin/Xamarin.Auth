using System;
using Xamarin.Forms;

namespace ComicBook
{
	public class App : Application
	{
		public App ()
		{
            MainPage = 
                // OK
                new MainPage()
                // Android only allows one navigation page on screen at a time 
                // new NavigationPage(new MainPage ())
                ;
		}
	}
}