using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using YesPojiQuota.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using YesPojiQuota.Utils;
using System.Diagnostics;
using Windows.UI.Core;

namespace YesPojiQuota.ViewModels
{
    public class AccountsViewModel : MainViewModel
    {
        private YesContext db;

        private bool _isLoaded = false;

        private ObservableCollection<AccountViewModel> _accounts;
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts ?? (_accounts = new ObservableCollection<AccountViewModel>());
            set => _accounts = value;
        }

        public AccountsViewModel(YesContext db, INavigationService ns) : base(ns)
        {
            this.db = db;
        }

        public override async Task Init()
        {
            await base.Init();

            if (!_isLoaded)
            {
                var accounts = db.Accounts.OrderBy(x=>x.Username).ToList();

                accounts.ForEach(x =>
                {
                    var acvm = new AccountViewModel(x);
                    acvm.Removed += AccountRemoved;
                    acvm.LightInit();
                    Accounts.Add(acvm);
                });

                await Task.Run(async () =>
                {
                    foreach (var acvm in Accounts)
                    {
                        await acvm.Init();
                    }
                });

                _isLoaded = true;

            }
        }

        private void AccountRemoved(object source)
        {
            Accounts.Remove(source as AccountViewModel);
        }

        public AccountViewModel CreateEmptyAccount()
        {
            var acc = new AccountViewModel();
            acc.OnSuccessOperation += o =>
            {
                Accounts.Add(o as AccountViewModel);
            };

            acc.Removed += AccountRemoved;

            return acc;
        }

        public async void RefreshQuota()
        {
            foreach (var a in Accounts)
            {
                //await Task.Run(() => a.Init());
                await a.Init();
            }
        }

    }


}
