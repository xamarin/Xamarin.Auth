using System;
using System.Collections.Generic;
using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;
using Xamarin.Forms;

namespace ComicBook
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = this;
            this.pickerUIFrameworks.ItemsSource = UIFrameworks;
            this.pickerViews.ItemsSource = Views;
            this.pickerNavigationType.ItemsSource = NavigationTypes;
            this.pickerFormsImplementations.ItemsSource = FormsImplementations;

            this.pickerUIFrameworks.SelectedIndexChanged += pickerUIFrameworks_SelectedIndexChanged;
            this.pickerFormsImplementations.SelectedIndexChanged += pickerFormsImplementations_SelectedIndexChanged;
            this.pickerNavigationType.SelectedIndexChanged += pickerNavigationType_SelectedIndexChanged;
            this.pickerViews.SelectedIndexChanged += pickerViews_SelectedIndexChanged;

            this.pickerUIFrameworks.SelectedIndex = 0;
            this.pickerFormsImplementations.SelectedIndex = 0;
            this.pickerNavigationType.SelectedIndex = 0;

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case "iOS":
                    this.pickerViews.SelectedIndex = 0;
                    break;
            }

            return;
        }


        public List<string> UIFrameworks => _UIFrameworks;

        List<string> _UIFrameworks = new List<string>()
        {
            "Embedded WebView",
            "Native UI (Custom Tabs or SFSafariViewController",
        };

        protected void pickerUIFrameworks_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            if (((string)p.SelectedItem).Equals("Native UI (Custom Tabs or SFSafariViewController"))
            {
                Settings.IsUsingNativeUI = true;
                pickerViews.IsEnabled = false;
            }
            else if (((string)p.SelectedItem).Equals("Embedded WebView"))
            {
                Settings.IsUsingNativeUI = false;
                pickerViews.IsEnabled = true;
            }
            else
            {
                throw new ArgumentException("UIFramework error");
            }

            return;
        }

        public List<string> FormsImplementations => _FormsImplementations;

        List<string> _FormsImplementations = new List<string>()
        {
            "Custom Renderers",
            "Presenters (Dependency Service/Injection)",
        };

        protected void pickerFormsImplementations_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            string implementation = ((string)p.SelectedItem);
            if (string.IsNullOrEmpty(implementation))
                return;

            if (implementation == "Presenters (Dependency Service/Injection)")
            {
                System.Diagnostics.Debug.WriteLine("Presenters (Dependency Service/Injection)");

                Settings.IsFormsImplementationRenderers = false;
                Settings.IsFormsImplementationPresenters = true;
            }
            else if (implementation == "Custom Renderers")
            {
                System.Diagnostics.Debug.WriteLine("Custom Renderers");

                Settings.IsFormsImplementationRenderers = true;
                Settings.IsFormsImplementationPresenters = false;
            }
            else
            {
                throw new ArgumentException("FormsImplementation error");
            }

            return;
        }

        public List<string> NavigationTypes => _NavigationTypes;

        List<string> _NavigationTypes = new List<string>()
        {
            "PushModalAsync",
            "PushAsync",
        };

        protected void pickerNavigationType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            string navigation_type = ((string)p.SelectedItem);
            if (string.IsNullOrEmpty(navigation_type))
                return;

            if (navigation_type == "PushAsync")
            {
                System.Diagnostics.Debug.WriteLine("PushAsync");

                Settings.IsFormsNavigationPushModal = false;
                Settings.IsFormsNavigationPush = true;
            }
            else if (navigation_type == "PushModalAsync")
            {
                System.Diagnostics.Debug.WriteLine("PushModalAsync");

                Settings.IsFormsNavigationPushModal = true;
                Settings.IsFormsNavigationPush = false;
            }
            else
            {
                throw new ArgumentException("NavigationTypes error");
            }

            return;
        }


        string web_view = null;

        public List<string> Views => _Views;

        List<string> _Views = new List<string>()
        {
            "UIWebView",
            "WKWebView",
        };

        protected void pickerViews_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker p = sender as Picker;

            web_view = ((string)p.SelectedItem);

            IEmbeddedWebViewConfiguration cfg = DependencyService.Get<IEmbeddedWebViewConfiguration>();

            if (null == cfg)
            {
                //TODO: check dependency service

                return;
            }

            if (web_view == "UIWebView")
            {
                System.Diagnostics.Debug.WriteLine("UIWebView");

                cfg.IsUsingWKWebView = false;
            }
            else if (web_view == "WKWebView")
            {
                System.Diagnostics.Debug.WriteLine("WKWebView");

                cfg.IsUsingWKWebView = true;
            }
            else
            {
                throw new ArgumentException("WebView error");
            }

            return;
        }

    }
}
