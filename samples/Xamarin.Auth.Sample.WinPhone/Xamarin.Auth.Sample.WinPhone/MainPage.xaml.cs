//
//  Copyright 2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Xamarin.Auth.Sample.WinPhone
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void OnClickFacebook (object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			this.facebookStatus.Text = App.LoginStatus;
		}
	}
}