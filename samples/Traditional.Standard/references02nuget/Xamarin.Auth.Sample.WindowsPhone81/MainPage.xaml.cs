﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Xamarin.Auth.Sample.Resources;

using System.Text;

using Xamarin.Auth.SampleData;

namespace Xamarin.Auth.Sample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            itemList.ItemsSource = null;
            itemList.ItemsSource = provider_list;

            return;
        }
    }
}