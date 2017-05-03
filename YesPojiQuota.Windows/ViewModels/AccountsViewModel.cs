using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Utils;
using System.Diagnostics;
using Windows.UI.Core;
using YesPojiQuota.Core.Models;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using YesPojiQuota.Core.Interfaces;

namespace YesPojiQuota.ViewModels
{
    public class AccountsViewModel : MainViewModel
    {
        private YesContext _db;
        private ILoginService _ls;

        private bool _isLoaded = false;

        private ObservableCollection<AccountViewModel> _accounts;
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts ?? (_accounts = new ObservableCollection<AccountViewModel>());
            set => _accounts = value;
        }

        public AccountsViewModel(YesContext db, ILoginService ls)
        {
            _db = db;
            _ls = ls;
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();

            if (!_isLoaded)
            {
                var accounts = _db.Accounts.OrderBy(x => x.Username).ToList();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
                {
                    accounts.ForEach(x =>
                    {
                        var acvm = CreateAccountViewModel(x);
                        acvm.LightInit();
                        DispatcherHelper.RunAsync(() => Accounts.Add(acvm));
                    });


                    foreach (var acvm in Accounts)
                    {
                        await acvm.InitAsync();
                    }
                });
#pragma warning restore CS4014

                _isLoaded = true;

            }
        }

        private void AccountRemoved(object source)
        {
            Accounts.Remove(source as AccountViewModel);
        }

        public AccountViewModel CreateAccountViewModel()
        {
            var acc = new AccountViewModel(_db, _ls);
            acc.OnSuccessOperation += o =>
            {
                Accounts.Add(o as AccountViewModel);
            };

            acc.Removed += AccountRemoved;

            return acc;
        }

        public AccountViewModel CreateAccountViewModel(Account a)
        {
            var acvm = new AccountViewModel(a, _db, _ls);
            acvm.Removed += AccountRemoved;

            return acvm;
        }

        public void RefreshQuota()
        {
            Task.Run(async () =>
            {
                foreach (var acvm in Accounts)
                {
                    await acvm.InitAsync();
                }
            });

        }

    }


}
