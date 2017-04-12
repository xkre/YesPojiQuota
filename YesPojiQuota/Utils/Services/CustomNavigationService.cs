using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using YesPojiQuota.Utils.Interfaces;

namespace YesPojiQuota.Utils.Services
{
    public class CustomNavigationService : INavigationService, ICustomNavigationService
    {
        private NavigationService nav;
        private Stack<string> navStack;

        public CustomNavigationService(NavigationService nav)
        {
            this.nav = nav;
            navStack = new Stack<string>();
        }

        public string CurrentPageKey
        {
            get
            {
                return nav.CurrentPageKey;
            }
        }


        public bool CanGoBack()
        {
            if (nav.CurrentPageKey != NavigationService.RootPageKey)
                return true;

            navStack.Clear();
            return false;
        }

        public void GoBack()
        {
            navStack.Pop();
            nav.GoBack();
            RefreshBackButtonVisibility();
        }

        public void Configure(string pageKey, Type pageType)
        {
            nav.Configure(pageKey, pageType);
        }

        public void NavigateTo(string pageKey)
        {
            navStack.Push(pageKey);
            nav.NavigateTo(pageKey);
            RefreshBackButtonVisibility();
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            navStack.Push(pageKey);
            nav.NavigateTo(pageKey, parameter);
            RefreshBackButtonVisibility();
        }

        private void RefreshBackButtonVisibility()
        {
            if (navStack.Count > 0)
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

        public bool GoBackSystem()
        {
            if (CanGoBack())
            {
                GoBack();
                return true;
            }

            return false;
        }
    }
}
