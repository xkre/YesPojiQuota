using Microsoft.Practices.ServiceLocation;
using System;
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
            SystemNavigationManager.GetForCurrentView().BackRequested += vm.BackRequested;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var avm = ServiceLocator.Current.GetInstance<AccountsViewModel>();

            AccountInputDialog accountInput = new AccountInputDialog();
            accountInput.DataContext = avm.CreateEmptyAccount();
            await accountInput.ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Messenger.Default.Register<string>(this, HandleNotification);
        }

        private async void HandleNotification(string a)
        {
            vm.NotiMessage = a;
            vm.NotiVisible = true;

            await Task.Delay(5000);

            vm.NotiVisible = false;
        }

        //protected override async void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    await vm.Init();
        //}
    }
}
