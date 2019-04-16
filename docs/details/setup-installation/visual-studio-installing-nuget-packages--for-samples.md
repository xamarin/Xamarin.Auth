# Visual Studio - Installing NuGet packages to samples

## Xamarin Traditional/Standard Samples 

Xamarin Traditional/Standard samples are samples in Xamarin.Android, Xamarin.iOS
and several Windows platforms (Windows Phone 8 Silverligh, Windows Phone 8.1 WinRT,
Windows 8.1 Store WinRT and Universal Windows Platform UWP)

The Providers solution is collection of 15+ providers with 30+ AOuth configurations.

The OAuth providers currently covered (2017-04):

*	google
*	Facebook
*	Twitter
*	Microsoft Live
*	GitHub
*	Instagram
*	Slack
*	MeetUp
*	StackOverflow	
*	Gitter
*	FitBit	
*	


## Xamarin.Forms Samples

### NativeUI samples Xamarin.Forms 

Xamarin.Forms with CustomRenderers implementation of Xamarin.Auth

Samples.NativeUI

#### Installing nugeta into Sample Projects

    Get-Project Samples.NativeUI            | Install-Package Xamarin.Auth
    Get-Project Samples.NativeUI.Android    | Install-Package Xamarin.Auth
    Get-Project Samples.NativeUI.iOS        | Install-Package Xamarin.Auth
    Get-Project Samples.NativeUI.UWP        | Install-Package Xamarin.Auth
    Get-Project Samples.NativeUI            | Install-Package Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.Android    | Install-Package Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.iOS        | Install-Package Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.UWP        | Install-Package Xamarin.Auth.XamarinForms


    Get-Project Samples.NativeUI            | Update-Package Xamarin.Auth
    Get-Project Samples.NativeUI.Android    | Update-Package Xamarin.Auth
    Get-Project Samples.NativeUI.iOS        | Update-Package Xamarin.Auth
    Get-Project Samples.NativeUI.UWP        | Update-Package Xamarin.Auth
    Get-Project Samples.NativeUI            | Update-Package Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.Android    | Update-Package Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.iOS        | Update-Package Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.UWP        | Update-Package Xamarin.Auth.XamarinForms


    Get-Project Samples.NativeUI            | Update-Package -IncludePrerelease Xamarin.Auth
    Get-Project Samples.NativeUI.Android    | Update-Package -IncludePrerelease Xamarin.Auth
    Get-Project Samples.NativeUI.iOS        | Update-Package -IncludePrerelease Xamarin.Auth
    Get-Project Samples.NativeUI.UWP        | Update-Package -IncludePrerelease Xamarin.Auth
    Get-Project Samples.NativeUI            | Update-Package -IncludePrerelease Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.Android    | Update-Package -IncludePrerelease Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.iOS        | Update-Package -IncludePrerelease Xamarin.Auth.XamarinForms
    Get-Project Samples.NativeUI.UWP        | Update-Package -IncludePrerelease Xamarin.Auth.XamarinForms

	
### Evolve16 samples - Xamarin.Forms 

Xamarin.Forms with Presenters (without CustomRenderers) implementation of Xamarin.Auth
