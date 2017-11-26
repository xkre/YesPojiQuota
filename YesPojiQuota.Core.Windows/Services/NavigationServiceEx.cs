using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace YesPojiQuota.Core.Windows.Services
{
    public class NavigationServiceEx : NavigationService
    {
        public override void NavigateTo(string pageKey, object parameter)
        {
            base.NavigateTo(pageKey, parameter);

            RefreshSystemBackButton();
        }

        public void RefreshSystemBackButton()
        {
            if (CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
