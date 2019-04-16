using System.Linq;
using LeanKit.Core.ApplicationServices;
using Xamarin.Forms;

namespace Xamarin.Auth.ANE
{
	public class App : Application
	{
		public App()
		{
			// The root page of your application
			var content = new ContentPage {
				Title = "Xamarin.Auth.ANE",
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};

			MainPage = new NavigationPage(content);
		}

		protected override void OnStart()
		{
			var accountProvider = TinyIoC.TinyIoCContainer.Current.Resolve<IAccountProvider>();

			var accounts = accountProvider.GetAll();

			if (null == accounts || accounts.Count() == 0)
			{
				accountProvider.Save(1, "ryan@leankit.com", "supersecret", LeanKit.Core.Enums.AuthorizationType.Basic,
									 new LeanKit.Core.Domain.OrganizationLink
									 {
										 HostName = "mobile-dogfood",
										 IsPasswordVerified = true,
										 Link = "https://mobile-dogfood.leankit.com/",
										 Text = "mobile-dogfood",
										 Title = "mobile-dogfood",
										 UserId = 1
									 });
			}
			else
			{
				foreach (LeanKit.Core.Domain.LeanKitAccount a in accounts)
				{
					System.Diagnostics.Debug.WriteLine("UserName = " + a.UserName);
				}
			}
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

