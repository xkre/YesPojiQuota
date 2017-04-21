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

        #region Properties
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

        public bool IsInitialized { get; protected set; } = false;
        #endregion Properties

        public override async Task InitAsync()
        {
            if (!IsInitialized)
            {
                await base.InitAsync();
                await _notiVM.InitAsync();

                await _accountsVM.InitAsync();

                await _ls.InitAsync();

                IsInitialized = true;
            }
        }

        public void RefreshAccounts()
        {
            _accountsVM.RefreshQuota();
        }

        private RelayCommand _navigateToSettings;
        public RelayCommand NavigateToSettings
        {
            get
            {
                return _navigateToSettings ?? (_navigateToSettings = new RelayCommand(() => _navigationService.NavigateTo(ViewModelKeys.SETTINGS_PAGE)));
            }
        }

        /* Unused Code
        public async void SendNotificationMessage()
        {
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
    }
}
