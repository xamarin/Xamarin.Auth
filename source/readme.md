# Xamarin.Auth 


Xamarin.Forms locks in a very specific version of the support libraries and google play services. 
You can't update them because Xamarin.Forms isn't compatible with them as it would need to be re-compiled.


When you want to update Xamarin.Forms ONLY update Xamarin.Forms, don't update all of the packages. When you update just Xamarin.Forms it will update it's dependencies to the correct version number needed.

If you already update the other nugets that aren't letting you update, you could simply uninstall your packages and re-install just Xamarin.Forms.

Here is a video: http://screencast.com/t/U5FBj6KlH


Script for update/reinstallation is based on

	Get-Project [[-Name] <string>] [-All]


script:

	get-project Xamarin.Auth.XamarinForms						| uninstall-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.Droid					| uninstall-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.iOS					| uninstall-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.iOS-Classic			| uninstall-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.WindowsPhone8			| uninstall-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.WindowsPhone81		| uninstall-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.WindowsStore81WinRT	| uninstall-package Xamarin.Forms


	get-project *.Auth.XamarinForms								| uninstall-package Xamarin.Forms

	get-project Xamarin.Auth.XamarinForms						| install-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.Droid					| install-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.iOS					| install-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.iOS-Classic			| install-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.WindowsPhone8			| install-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.WindowsPhone81		| install-package Xamarin.Forms
	get-project Xamarin.Auth.XamarinForms.WindowsStore81WinRT	| install-package Xamarin.Forms

	get-project *.Auth.XamarinForms								| install-package Xamarin.Forms


	get-project *.Auth.XamarinForms								| install-package Xamarin.Forms

	https://forums.xamarin.com/discussion/57283/unable-to-find-a-version-of-xf-compatible-with



	get-project Xamarin.Auth.LinkSource							| uninstall-package PCLCrypto
	get-project Xamarin.Auth.Portable							| uninstall-package PCLCrypto
	get-project Xamarin.Auth.UniversalWindowsPlatform			| uninstall-package PCLCrypto
	get-project Xamarin.Auth.Windows81Universal					| uninstall-package PCLCrypto
	get-project Xamarin.Auth.WindowsPhone8						| uninstall-package PCLCrypto
	get-project Xamarin.Auth.WindowsPhone81						| uninstall-package PCLCrypto
	get-project Xamarin.Auth.WinRTWindows81						| uninstall-package PCLCrypto
	get-project Xamarin.Auth.WinRTWindowsPhone81				| uninstall-package PCLCrypto
	get-project Xamarin.Auth.XamarinAndroid						| uninstall-package PCLCrypto
	get-project Xamarin.Auth.XamarinIOS							| uninstall-package PCLCrypto
	get-project Xamarin.Auth.XamarinIOS-Classic					| uninstall-package PCLCrypto

	get-project Xamarin.Auth.LinkSource							| install-package PCLCrypto
	get-project Xamarin.Auth.Portable							| install-package PCLCrypto
	get-project Xamarin.Auth.UniversalWindowsPlatform			| install-package PCLCrypto
	get-project Xamarin.Auth.Windows81Universal					| install-package PCLCrypto
	get-project Xamarin.Auth.WindowsPhone8						| install-package PCLCrypto
	get-project Xamarin.Auth.WindowsPhone81						| install-package PCLCrypto
	get-project Xamarin.Auth.WinRTWindows81						| install-package PCLCrypto
	get-project Xamarin.Auth.WinRTWindowsPhone81				| install-package PCLCrypto
	get-project Xamarin.Auth.XamarinAndroid						| install-package PCLCrypto
	get-project Xamarin.Auth.XamarinIOS							| install-package PCLCrypto
	get-project Xamarin.Auth.XamarinIOS-Classic					| install-package PCLCrypto
