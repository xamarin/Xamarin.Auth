using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ComicBook
{
    public partial class MasterPage : ContentPage
    {
        List<string> menu = new List<string>()
		{
			"OAuth Settings",
			"Evolve16 ComicBook Sample",
			"OAuth Provider Samples",
            "OAuth Provider Quick Samples",
            "UserAgent tests",
		};

        public MasterPage()
        {
            InitializeComponent();

            ListView.ItemsSource = menu;
            ListView.ItemSelected += ListView_ItemSelected;
            return;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedItemChanged(sender, e);

            return;
        }

        public delegate void SelectedItemChangedEventHandler(object sender, SelectedItemChangedEventArgs e);
        public event SelectedItemChangedEventHandler SelectedItemChanged;

    }
}
