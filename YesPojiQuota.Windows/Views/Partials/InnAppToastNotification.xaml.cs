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
using YesPojiQuota.Core.ViewModels;
using YesPojiQuota.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YesPojiQuota.Views.Partials
{
    public sealed partial class InnAppToastNotification : UserControl
    {
        private bool _isAnimating;

        public StatusViewModel Vm => (StatusViewModel)DataContext;

        public InnAppToastNotification()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnimating)
                return;

            _isAnimating = true;

            if (SessionGrid.Visibility == Visibility.Collapsed)
            {
                SessionGridEnter.Begin();
            }
            else
            {
                SessionGridExit.Begin();
            }
        }

        private void AnimationCompleted(object sender, object e)
        {
            _isAnimating = false;
        }
    }
}
