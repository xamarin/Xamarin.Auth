using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using Xamarin.Auth;


namespace ComicBook
{
    public partial class ProvidersSamplesPage : TabbedPage
    {
        protected Xamarin.Auth.WebAuthenticator authenticator = null;


        List<string> ProviderNames = null;

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
            
            listViewProviders.ItemSelected += ProvidersList_Handle_ItemSelected;
            listViewProviderSamples.ItemSelected += ProviderSamplesList_Handle_ItemSelected;

            //BindingContext = this;
            listViewProviders.ItemsSource = ProviderNames;

            return;
        }

        string Provider = null;
        List<Xamarin.Auth.ProviderSamples.Helpers.OAuth> ProviderTestCases = null;

        protected void ProvidersList_Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ListView ItemSelected");

            listViewProviderSamples.ItemsSource = null;
            string selection = (string)e.SelectedItem;

            System.Diagnostics.Debug.WriteLine($"    Provider ");
            System.Diagnostics.Debug.WriteLine($"        Selection = {selection}");

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

        protected void ProviderSamplesList_Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Type selection_type = e.SelectedItem.GetType();
            string selection = selection_type.ToString().Replace("Xamarin.Auth.ProviderSamples.","");

            System.Diagnostics.Debug.WriteLine($"    Provider Sample");
            System.Diagnostics.Debug.WriteLine($"        Selection = {selection}");
        	
            Xamarin.Auth.WebAuthenticator authenticator = null;

            Xamarin.Auth.ProviderSamples.Helpers.OAuth1 oauth1 = null;
            Xamarin.Auth.ProviderSamples.Helpers.OAuth2 oauth2 = null;

            oauth2 = Activator.CreateInstance(selection_type) as Xamarin.Auth.ProviderSamples.Helpers.OAuth2;

            if (null == oauth2)
            { 
                oauth1 = Activator.CreateInstance(selection_type) as Xamarin.Auth.ProviderSamples.Helpers.OAuth1;

                if(null == oauth1)
                {
                    throw new ArgumentException("Not OAuth object!");
                }

                authenticator = Map(oauth1);
            }

            authenticator = Map(oauth2);

            return;
        }

        public OAuth2Authenticator Map(Xamarin.Auth.ProviderSamples.Helpers.OAuth2 oauth2)
        {
            OAuth2Authenticator o2a = new OAuth2Authenticator
                (
                    clientId: oauth2.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    clientSecret: oauth2.OAuth_SecretKey_ConsumerSecret_APISecret,
                    scope: oauth2.OAuth2_Scope, 
                    authorizeUrl: oauth2.OAuth_UriAuthorization,
                    accessTokenUrl: oauth2.OAuth_UriCallbackAKARedirect,
                    redirectUrl: oauth2.OAuth_UriCallbackAKARedirect,
                    getUsernameAsync: null,
                    isUsingNativeUI: true
                );

            System.Diagnostics.Debug.WriteLine(o2a.ToString());

            return o2a;
        }

        public OAuth1Authenticator Map(Xamarin.Auth.ProviderSamples.Helpers.OAuth1 oauth1)
        {
            OAuth1Authenticator o1a = null; //new OAuth1Authenticator();

            System.Diagnostics.Debug.WriteLine(o1a?.ToString());

            return o1a;
        }
    }
}
