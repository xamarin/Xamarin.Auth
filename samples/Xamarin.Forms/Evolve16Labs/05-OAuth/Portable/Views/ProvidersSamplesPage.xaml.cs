using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ComicBook
{
    public partial class ProvidersSamplesPage : ContentPage
    {
        protected Xamarin.Auth.WebAuthenticator authenticator = null;

        List<string> Providers = new List<string>()
        {
            "Google",
            "FaceBook",
            "MeetUp",
            "LinkedIn",
        };

        public ProvidersSamplesPage()
        {
            InitializeComponent();

            buttonGoogle.Clicked += ButtonGoogle_Clicked;
            buttonFacebook.Clicked += ButtonFacebook_Clicked;
            buttonLinkedIn.Clicked += ButtonLinkedIn_Clicked;
            buttonMeetUp.Clicked += ButtonMeetUp_Clicked;

            BindingContext = this;
            listViewProviders.ItemsSource = Providers;
            listViewProviders.ItemTapped += Handle_ItemTapped;
            listViewProviders.ItemSelected += Handle_ItemSelected;

            return;
        }
    }
}
