﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using YesPojiQuota.ViewModels;
using YesPojiQuota.Views.Partials;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using YesPojiQuota.Core.Windows.Services;
using GalaSoft.MvvmLight.Ioc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YesPojiQuota.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel vm { get { return DataContext as MainPageViewModel; } }

        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var avm = SimpleIoc.Default.GetInstance<AccountsPanelViewModel>();

            AccountInputDialog accountInput = new AccountInputDialog();
            accountInput.DataContext = avm.CreateAccountViewModel();
            await accountInput.ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            vm.InitAsync();
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Windows.System.VirtualKey.R)
            {
                var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(Windows.System.VirtualKey.Control);

                if (ctrlState != CoreVirtualKeyStates.None)
                {
                    vm.RefreshAccounts();

                    e.Handled = true;
                }
            }
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            var nav = ViewModelLocator.NavigationService as NavigationServiceEx;

            if (nav.CanGoBack)
            {
                nav.GoBack();
                nav.RefreshSystemBackButton();
                e.Handled = true;
            }
        }
    }
}
