using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;

namespace ComicBook
{
    public partial class ProvidersSamplesPage : TabbedPage
    {
        protected Xamarin.Auth.WebAuthenticator authenticator = null;


        List<string> ProviderNames = new List<string>()
        {
            "Google",
            "FaceBook",
            "MeetUp",
            "LinkedIn",
        };

        List<Xamarin.Auth.ProviderSamples.Helpers.OAuth> Providers = null;

        public ProvidersSamplesPage()
        {
            InitializeComponent();

            Providers = 
                (
                    from provider in Xamarin.Auth.ProviderSamples.Data.TestCases.Values.ToList()
                        orderby 
                            provider.OrderUI                    
                        select
                            provider
                ).ToList()
                ;

            ProviderNames =
                (
                    from provider in Providers
                        select
                            provider.ProviderName
                ).Distinct().ToList()
                 ;
            
            listViewProviders.ItemTapped += Handle_ItemTapped;
            listViewProviders.ItemSelected += Handle_ItemSelected;

            //BindingContext = this;
            listViewProviders.ItemsSource = ProviderNames;

            return;
        }

        protected void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            #if DEBUG
            System.Diagnostics.Debug.WriteLine("ListView ItemTapped");
            #endif
        	
            return;
        }

        string Provider = null;
        List<Xamarin.Auth.ProviderSamples.Helpers.OAuth> ProviderTestCases = null;

        protected void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            #if DEBUG
            System.Diagnostics.Debug.WriteLine("ListView ItemSelected");
            #endif

            listViewProviderSamples.ItemsSource = null;
            string selection = (string)e.SelectedItem;

            Provider = selection;

            ProviderTestCases = 
                (
                    from provider in Xamarin.Auth.ProviderSamples.Data.TestCases.Values.ToList()
                        where 
                            provider.ProviderName == Provider
						select
							provider
				).ToList()
				;

            listViewProviderSamples.ItemsSource = ProviderTestCases;

            return;
        }

    }
}
