using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YesPojiQuota.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel Vm => (SettingsViewModel)DataContext;
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var source = sender as TextBox;

            if (Regex.IsMatch(source.Text, @"\D"))
            {
                source.Text = Regex.Replace(source.Text, @"\D", "");
            }
        }
    }
}
