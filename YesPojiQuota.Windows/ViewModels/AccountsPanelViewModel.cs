using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Utils;

namespace YesPojiQuota.ViewModels
{
    public class AccountsPanelViewModel : MainViewModel
    {
        private YesContext _db;

        private bool _isLoaded = false;

        private ObservableCollection<AccountViewModel> _accounts;
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts ?? (Accounts = new ObservableCollection<AccountViewModel>());
            set => Set("Accounts", ref _accounts, value);
        }

        public AccountsPanelViewModel(YesContext db)
        {
            _db = db;
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();

            if (!_isLoaded)
            {
                var accounts = _db.Accounts
                                  .Include(a => a.Quota)
                                  .OrderBy(a => a.Username);

                _isLoaded = true;

                foreach (var a in accounts)
                {
                    var acvm = CreateAccountViewModel(a);
                    DispatcherHelper.RunAsync(() => Accounts.Add(acvm));
                    await acvm.InitAsync();
                }
            }
        }

        public AccountViewModel CreateAccountViewModel()
        {
            var acvm = ServiceLocator.Current.GetInstance<AccountViewModel>(Guid.NewGuid().ToString());

            acvm.OnSuccessOperation += async o =>
            {
                var acvm2 = (AccountViewModel)o;

                Accounts.Add(acvm2);

                acvm.Removed += AccountRemoved;

                await acvm2.InitAsync();
                await acvm2.RefreshDataAsync();
                await acvm2.SaveData();

                acvm2.RefreshCanConnectProperty();
            };
            return acvm;
        }

        public AccountViewModel CreateAccountViewModel(Account a)
        {
            var acvm = ServiceLocator.Current.GetInstance<AccountViewModel>(a.AccountId.ToString());
            acvm.Account = a;
            acvm.Removed += AccountRemoved;

            if (a.Quota == null)
            {
                //Should not happen - done as precaution
                Debug.WriteLine(@"Debug:::: Entered a.Quota == null in 
                            CreateAccountViewModel ::: This should not happen");
                a.Quota = new Quota() { AccountId = a.AccountId };
            }

            return acvm;
        }

        private void AccountRemoved(object source)
        {
            Accounts.Remove((AccountViewModel)source);
        }

        public void RefreshQuota()
        {
            Task.Run(async () =>
            {
                MessengerInstance.Send(new LoadingMessage()
                    { IsLoading = true, Message = "Refreshing Quota" }
                );
                foreach (var acvm in Accounts)
                {
                    await acvm.RefreshDataAsync();
                    await acvm.SaveDataIfNecessary();
                }

                MessengerInstance.Send(new LoadingMessage()
                    { IsLoading = false, Message = "Quota Refreshed" }
                );
            });
        }

        public void Sort(SortType sorting = SortType.Quota)
        {
            switch (sorting)
            {
                case SortType.AccountName:
                    Accounts = new ObservableCollection<AccountViewModel>(Accounts.ToList().OrderBy(a => a.Account.AccountId)); 
                    break;
                default:
                    Accounts = new ObservableCollection<AccountViewModel>(Accounts.ToList().OrderBy(a => a.Quota.Available));
                    break;
            }
        }

        public enum SortType
        {
            Quota, 
            AccountName,
        }
    }
}
