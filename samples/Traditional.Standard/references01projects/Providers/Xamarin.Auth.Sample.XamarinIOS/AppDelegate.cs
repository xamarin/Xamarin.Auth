﻿using System;
using System.Collections.Generic;
using System.Linq;

#if ! __CLASSIC__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace Xamarin.Auth.Sample.XamarinIOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
			
			// If you have defined a root view controller, set it here:
			var navCtrl = new UINavigationController (new TestProvidersController());
			navCtrl.NavigationBar.BarTintColor = new UIColor (0x2C/255f, 0x3E/255f, 0x50/255f, 1);
			navCtrl.NavigationBar.TitleTextAttributes = new UIStringAttributes {ForegroundColor = UIColor.White};
			window.RootViewController = navCtrl;
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

