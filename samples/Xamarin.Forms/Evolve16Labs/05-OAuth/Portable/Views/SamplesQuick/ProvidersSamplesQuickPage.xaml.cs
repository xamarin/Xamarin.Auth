using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ComicBook
{
    public partial class ProvidersSamplesQuickPage : ContentPage
    {
        public ProvidersSamplesQuickPage()
        {
            InitializeComponent();

            buttonGoogle.Clicked += ButtonGoogle_Clicked;
            buttonFacebook.Clicked += ButtonFacebook_Clicked;
            buttonLinkedIn.Clicked += ButtonLinkedIn_Clicked;
            buttonMeetUp.Clicked += ButtonMeetUp_Clicked;

            return;
        }

        protected void ButtonGoogle_Clicked(object sender, EventArgs e)
        {
            Google();

            return;
        }

        protected void ButtonFacebook_Clicked(object sender, EventArgs e)
        {
            Facebook();

            return;
        }

        protected void ButtonLinkedIn_Clicked(object sender, EventArgs e)
        {
            LinkedIn();

            return;
        }

        protected void ButtonMeetUp_Clicked(object sender, EventArgs e)
        {
            MeetUp();

            return;
        }
    }
}
