using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YesPojiQuota.Views.Partials
{
    public sealed partial class AccountsPanel : UserControl
    {
        public AccountsPanelViewModel Vm  => (AccountsPanelViewModel)DataContext; 

        public AccountsPanel()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            Vm.Sort();
        }

        /*
        private async void DeleteContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog a = new ContentDialog();

            a.Title = "Confirmation";
            a.Content = "Are you sure to delete this?";

            a.PrimaryButtonText = "Yes";
            a.SecondaryButtonText = "No";

            var source = e.OriginalSource as AccountViewModel;

            await Task.Delay(100);

            //await Task.Delay(100);
            //FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

            //FrameworkElement senderElement = sender as FrameworkElement;
            //FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            //flyoutBase.ShowAt(senderElement.Parent as FrameworkElement);
        }*/
    }
}
