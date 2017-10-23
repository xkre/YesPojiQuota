using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using YesPojiQuota.CoreLibs.Interfaces;
using YesPojiQuota.CoreLibs.Data;
using Microsoft.EntityFrameworkCore;
using YesPojiQuota.CoreLibs.Observers;
using System.Diagnostics;

namespace YesPojiQuota.CoreLibs.ViewModels
{
    public class MainPageViewModel : MainViewModel
    {
        private AccountsPanelViewModel _accountsVM;
        private InnAppToastViewModel _inAppToastVM;

        private ILoginService _ls;
        private INetworkService _ns;
        private IDialogService _ds;
        private YesContext _db;
        private NetworkChangeHandler _nch;
        private YesSessionUpdater _ysu;

        private bool _yesConnected = false;

        public MainPageViewModel(
            INavigationService navS,
            ILoginService ls,
            INetworkService ns,
            IDialogService ds,
            AccountsPanelViewModel acsvm,
            InnAppToastViewModel iatvm,
            NetworkChangeHandler nch,
            YesSessionUpdater ysu,
            YesContext db)
            : base(navS)
        {
            _ls = ls;
            _ns = ns;
            _ds = ds;
            _db = db;
            _nch = nch;
            _ysu = ysu;

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
                await Task.Run(async () =>
                {
                    Stopwatch s1 = new Stopwatch();
                    s1.Start();

                    await base.InitAsync();

                    InitDatabase();
                    InitYesConnectionEventsHandler();

                    await _accountsVM.InitAsync();
                    await _inAppToastVM.InitAsync();

                    _ysu.Init();
                    _nch.Init();

                    IsInitialized = true;
                    s1.Stop();

                    Debug.WriteLine($"Main Init completed in: {s1.Elapsed}");
                });
            }
        }

        private void InitYesConnectionEventsHandler()
        {
            _nch.YesConnected += () =>
            {
                _yesConnected = true;
                RefreshAccounts();

                _ls.InitAsync();
            };
            _nch.YesDisconnected += () => _yesConnected = false;
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

        public void RefreshAccounts()
        {
            if (_yesConnected)
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
