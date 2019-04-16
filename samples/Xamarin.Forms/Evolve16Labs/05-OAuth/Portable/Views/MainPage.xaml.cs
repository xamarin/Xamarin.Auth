using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using ComicBook;
using System.Text;
using Xamarin.Auth.XamarinForms;

namespace ComicBook
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            /*
            store = AccountStore.Create();
            account = store.FindAccountsForService(ServiceId).FirstOrDefault();

            if (account != null)
            {
                statusText.Text = "Restored previous session";
                getProfileButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            }
            */
            this.BindingContext = this;

            this.masterPage.SelectedItemChanged += MasterPage_SelectedItemChanged;

            return;
        }

        private void MasterPage_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            string selected_item = (string)e.SelectedItem;

            switch (selected_item)
            {
                case "OAuth Settings":
                    this.IsPresented = false;
                    this.Detail = new NavigationPage(new SettingsPage());
                    break;
                case "Evolve16 ComicBook Sample":
                    this.IsPresented = false;
                    this.Detail = new NavigationPage(new Evolve16SamplePage());
                    break;
                case "OAuth Provider Samples":
                    this.IsPresented = false;
                    this.Detail = new NavigationPage(new ProvidersSamplesPage());
                    break;
                case "OAuth Provider Quick Samples":
                    this.IsPresented = false;
                    this.Detail = new NavigationPage(new ProvidersSamplesQuickPage());
                    break;
                case "UserAgent tests":
                    this.IsPresented = false;
                    this.Detail = new NavigationPage(new UserAgentTestPage());
                    break;
                default:
                    throw new ArgumentException("No way!");
            }
            return;
        }
    }
}