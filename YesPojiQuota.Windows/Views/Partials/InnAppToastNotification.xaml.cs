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
using YesPojiQuota.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YesPojiQuota.Views.Partials
{
    public sealed partial class InnAppToastNotification : UserControl
    {
        public InnAppToastViewModel Vm => (InnAppToastViewModel)DataContext;

        public InnAppToastNotification()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SessionGrid.Visibility == Visibility.Collapsed)
                SessionGrid.Visibility = Visibility.Visible;
            else
                SessionGrid.Visibility = Visibility.Collapsed;
        }
    }
}
