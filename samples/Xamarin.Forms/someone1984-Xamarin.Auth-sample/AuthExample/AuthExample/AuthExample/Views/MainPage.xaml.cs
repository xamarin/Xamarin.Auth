using AuthExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        MainPageViewModel mainPageViewModel;
        public MainPage ()
		{
			InitializeComponent ();
            BindingContext = mainPageViewModel = new MainPageViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainPageViewModel.OnPageAppearingCommand.Execute(null);
        }
    }
}