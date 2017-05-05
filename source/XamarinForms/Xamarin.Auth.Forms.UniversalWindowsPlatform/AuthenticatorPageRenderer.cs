using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:
    ExportRenderer
        (
            typeof(Xamarin.Auth.XamarinForms.AuthenticatorPage),
            typeof(Xamarin.Auth.XamarinForms.UniversalWindowsPlatform.AuthenticatorPageRenderer)
        )
]
namespace Xamarin.Auth.XamarinForms.UniversalWindowsPlatform
{
    public class AuthenticatorPageRenderer : Xamarin.Forms.Platform.UWP.PageRenderer
    {
        private Xamarin.Auth.Authenticator _authenticator;
        private Windows.UI.Xaml.Controls.Frame _frame;

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_frame != null)
                    {
                        _frame.NavigationFailed -= Frame_NavigationFailed;
                        _frame.Navigated -= Frame_Navigated;
                        _frame.Navigating -= Frame_Navigating;
                        _frame.NavigationStopped -= Frame_NavigationStopped;
                        _frame = null;
                    }

                    if (_authenticator != null)
                    {
                        _authenticator.Completed -= Authenticator_Completed;
                        _authenticator.Error -= Authenticator_Error;
                        _authenticator = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Xamarin.Auth.AuthException("UWP Dispose");
            }

            base.Dispose(disposing);
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
        {
            try
            {
                base.OnElementChanged(e);

                System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer.OnElementChanged");

                if (e == null)
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: e = {null}");
                else
                {
                    if (e.NewElement == null)
                        System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: e.NewElement = {null}");
                    if (e.OldElement == null)
                        System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: e.OldElement = {null}");
                }

                if (Element == null)
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: Element is {null}");
                else
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: Element is " + Element);

                if (Control == null)
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: Control is {null}");
                else
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: Control is " + Control);

                if (Control == null)
                {
                    WindowsPage windowsPage = new WindowsPage();

                    _frame = windowsPage.Frame;
                    if (_frame == null)
                    {
                        _frame = new Windows.UI.Xaml.Controls.Frame
                        {
                            Language = global::Windows.Globalization.ApplicationLanguages.Languages[0]
                        };

                        _frame.NavigationFailed += Frame_NavigationFailed;
                        _frame.Navigated += Frame_Navigated;
                        _frame.Navigating += Frame_Navigating;
                        _frame.NavigationStopped += Frame_NavigationStopped;

                        windowsPage.Content = _frame;
                        SetNativeControl(windowsPage);
                    }

                    _authenticator.Completed -= Authenticator_Completed;
                    _authenticator.Completed += Authenticator_Completed;
                    _authenticator.Error -= Authenticator_Error;
                    _authenticator.Error += Authenticator_Error;

                    Type pageType = _authenticator.GetUI();
                    _frame.Navigate(pageType, _authenticator);
                    Windows.UI.Xaml.Window.Current.Activate();
                }
            }
            catch (Exception ex)
            {
                throw new Xamarin.Auth.AuthException("UWP OnElementChanged");
            }

            return;
        }

        private void Frame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: In UWP Frame_Navigating");

            switch (e.NavigationMode)
            {
                case Windows.UI.Xaml.Navigation.NavigationMode.Back:
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: NavigationMode = Back");
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.Forward:
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: NavigationMode = Forward");
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.New:
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: NavigationMode = New");
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.Refresh:
                    System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: NavigationMode = Refresh");
                    break;
            }
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: Cancel = " + e.Cancel);
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: SourcePageType = " + e.SourcePageType.ToString());

            return;
        }

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: In UWP Frame_Navigated");

            return;
        }

        private async void Frame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: In UWP Frame_NavigationFailed");

            return;
        }

        private async void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: In UWP Authenticator_Error");

            return;
        }

        private async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: In UWP Authenticator_Completed");
        }

        private void Frame_NavigationStopped(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticatorPageRenderer: In UWP Frame_Stopped");
        }
    }
}

