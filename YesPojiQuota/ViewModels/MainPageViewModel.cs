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
using YesPojiQuota.Utils.Interfaces;

namespace YesPojiQuota.ViewModels
{
    public class MainPageViewModel : MainViewModel
    {
        private AccountsViewModel _accountsVM = ServiceLocator.Current.GetInstance<AccountsViewModel>();

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public async void SendNotificationMessage()
        {
            string key = String.Empty;
            var login = ServiceLocator.Current.GetInstance<ILoginService>();

            var isLoggedIn = await login.IsOnline();

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
            Task.Run(() =>
            {
                _accountsVM.RefreshQuota();
            });
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
