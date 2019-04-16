using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Xamarin.Auth.Sample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Invoked when the application is launched through a custom URI scheme, such as
        /// is the case in an OAuth 2.0 authorization flow.
        /// </summary>
        /// <param name="args">Details about the URI that activated the app.</param>
        protected override void OnActivated(IActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
            }
            Window.Current.Content = rootFrame;

            // When the app was activated by a Protocol (custom URI scheme), forwards
            // the URI to the MainPage through a Navigate event.
            if (args.Kind == ActivationKind.Protocol)
            {
                // Extracts the authorization response URI from the arguments.
                ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)args;
                Uri uri = protocolArgs.Uri;
                System.Diagnostics.Debug.WriteLine("Authorization Response: " + uri.AbsoluteUri);

                // Gets the current frame, making one if needed.
                Frame frame = Window.Current.Content as Frame;
                if (frame == null)
                {
                    frame = new Frame();
                }

                // Opens the URI for "navigation" (handling) on the MainPage.
                frame.Navigate(typeof(MainPage), uri);
                Window.Current.Content = frame;
                Window.Current.Activate();
            }



            /*
             * 2nd version
            if (args.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                rootFrame.Navigate(typeof(MainPage), protocolArgs.Uri.AbsolutePath);
            }
            */
            Window.Current.Activate();
        }
    }
}
