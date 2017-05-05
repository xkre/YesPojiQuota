using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Enums;
using YesPojiQuota.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace YesPojiQuota.ViewModels
{
    public class MainPageViewModel : MainViewModel
    {
        private AccountsViewModel _accountsVM;
        private InnAppToastViewModel _inAppToastVM;

        private ILoginService _ls;
        private INetworkService _ns;
        private IDialogService _ds;
        private YesContext _db;

        public MainPageViewModel(
            INavigationService navS,
            ILoginService ls,
            INetworkService ns,
            IDialogService ds,
            AccountsViewModel acsvm,
            InnAppToastViewModel iatvm,
            YesContext db)
            : base(navS)
        {
            _ls = ls;
            _ns = ns;
            _ds = ds;
            _db = db;

            _accountsVM = acsvm;
            _inAppToastVM = iatvm;
        }

        #region Properties
        public bool IsInitialized { get; protected set; } = false;
        #endregion Properties

        public override async Task InitAsync()
        {
            if (!IsInitialized)
            {
                await base.InitAsync();
                InitDatabase();


                await _inAppToastVM.InitAsync();
                await _accountsVM.InitAsync();
                await _ls.InitAsync();

                IsInitialized = true;
            }
        }

        private void InitDatabase()
        {
            try
            {
                _db.Database.Migrate();
            }
            catch
            {
                var quotas = _db.Quotas.ToList();
                var accounts = _db.Accounts.ToList();

                try
                {
                    _db.Database.EnsureDeleted();

                    _db.Database.Migrate();
                    _db.Accounts.AddRange(accounts);
                    _db.Quotas.AddRange(quotas);

                    _db.SaveChanges();
                }
                catch
                {
                    _db.Database.EnsureDeleted();
                    _db.Database.Migrate();

                    _ds.ShowError("There was an error during database migration", "Database Migration Error", "OK", () => { });
                }
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
