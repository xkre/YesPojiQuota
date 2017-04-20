using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using YesPojiQuota.Utils;
using GalaSoft.MvvmLight.Command;
using YesPojiQuota.Core.Interfaces;

namespace YesPojiQuota.ViewModels
{
    public class MainPageViewModel : MainViewModel
    {
        private AccountsViewModel _accountsVM = ServiceLocator.Current.GetInstance<AccountsViewModel>();
        private InnAppToastViewModel _notiVM = ServiceLocator.Current.GetInstance<InnAppToastViewModel>();

        private ILoginService _ls;

        public MainPageViewModel(INavigationService navigationService, ILoginService ls) : base(navigationService)
        {
            _ls = ls;
        }

        /* Unused Code
        public async void SendNotificationMessage()
        {
            string key = String.Empty;
            var login = ServiceLocator.Current.GetInstance<ILoginService>();

            var isLoggedIn = await login.LoginPortalAvailable();

            if (!isLoggedIn)
            {
                if (login.CanLogin())
                {
                    key = login.GetKey();
                    CanLogin = true;
                }
                else
                    CanLogin = false;
            }

            key = isLoggedIn ? isLoggedIn.ToString() : login.GetKey();

            Messenger.Default.Send(
                new NotificationMessageAction<string>(
                    key,
                    reply =>
                    {
                        PageTitle = reply;
                    }
                )
            );
        }
        */

        public override async Task InitAsync()
        {
            await base.InitAsync();
            await _notiVM.InitAsync();

            await _ls.InitAsync();
        }


        private bool _notiVisible;
        public bool NotiVisible
        {
            get { return _notiVisible; }
            set { Set("NotiVisible", ref _notiVisible, value); }
        }

        private string _notiMessage;
        public string NotiMessage
        {
            get { return _notiMessage; }
            set { Set("NotiMessage",ref  _notiMessage , value); }
        }



        public void RefreshAccounts()
        {
            _accountsVM.RefreshQuota();
        }

        //private RelayCommand _refreshAccounts;
        //public RelayCommand RefreshAccounts
        //{
        //    get
        //    {
        //        return _refreshAccounts ?? (_refreshAccounts = new RelayCommand(() =>
        //          {
        //              _accountsVM.RefreshQuota();
        //          }
        //        ));
        //    }
        //}


        private RelayCommand _navigateToSettings;
        public RelayCommand NavigateToSettings
        {
            get
            {
                return _navigateToSettings ?? (_navigateToSettings = new RelayCommand(() => _navigationService.NavigateTo(ViewModelKeys.SETTINGS_PAGE)));
            }
        }

    }
}
