using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Data;
using Microsoft.EntityFrameworkCore;
using YesPojiQuota.Core.Observers;
using System.Diagnostics;
using YesPojiUtmLib.Services;
using YesPojiQuota.Core.Utils;

namespace YesPojiQuota.Core.ViewModels
{
    public class MainPageViewModel : MainViewModel
    {
        private AccountsPanelViewModel _accountsVM;
        private StatusViewModel _inAppToastVM;

        private IYesLoginService _ls;
        private IYesNetworkService _ns;
        private IDialogService _ds;
        private YesContext _db;
        private NetworkChangeHandler _nch;
        private YesSessionUpdater _ysu;

        private bool _yesConnected = false;

        public MainPageViewModel(
            INavigationService navS,
            IYesLoginService ls,
            IYesNetworkService ns,
            IDialogService ds,
            AccountsPanelViewModel acsvm,
            StatusViewModel iatvm,
            NetworkChangeHandler nch,
            YesSessionUpdater ysu,
            YesContext db)
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

        public void NavigateToSettings()
        {
            NavigationService.NavigateTo(ViewModelKeys.SETTINGS_PAGE);
        }
    }
}
