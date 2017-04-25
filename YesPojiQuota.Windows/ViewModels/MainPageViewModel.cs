using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using YesPojiQuota.Utils;
using GalaSoft.MvvmLight.Command;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Enums;

namespace YesPojiQuota.ViewModels
{
    public class MainPageViewModel : MainViewModel
    {
        private AccountsViewModel _accountsVM;
        private InnAppToastViewModel _inAppToastVM;

        private ILoginService _ls;
        private INetworkService _ns;

        public MainPageViewModel(
            INavigationService navS, 
            ILoginService ls, 
            INetworkService ns,
            AccountsViewModel acsvm,
            InnAppToastViewModel iatvm) 
            : base(navS)
        {
            _ls = ls;
            _ns = ns;

            _accountsVM = acsvm;
            _inAppToastVM = iatvm;
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
                await _inAppToastVM.InitAsync();

                await _accountsVM.InitAsync();

                await _ls.InitAsync();

                IsInitialized = true;
            }
        }

        public async void RefreshAccounts()
        {
            if (_ns.NetworkType == NetworkCondition.NotConnected || _ns.NetworkType == NetworkCondition.OnlineNotYes)
            {
                _inAppToastVM.InitLoading();
                if (await _ns.CheckConnectionAsync())
                {
                    _accountsVM.RefreshQuota();
                }
            }
            else
            {
                _accountsVM.RefreshQuota();
                Messenger.Default.Send("Quota Refreshed");
            }
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
