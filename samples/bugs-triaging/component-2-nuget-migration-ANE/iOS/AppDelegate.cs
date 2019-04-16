using Foundation;
using LeanKit.Core;
using LeanKit.Core.ApplicationServices;
using LeanKit.iOS.Settings;
using LeanKit.UI.Application;
using TinyIoC;
using UIKit;

namespace Xamarin.Auth.ANE.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			var container = TinyIoCContainer.Current;

			container.Register(typeof(AccountStore), AccountStore.Create());
			container.Register(typeof(IApplicationSettings), new ApplicationSettings());
			container.Register<IAccountProvider, AccountProvider>();

			var applicationSettings = container.Resolve<IApplicationSettings>();


			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}

