﻿using GalaSoft.MvvmLight.Threading;
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
    public class AccountsViewModel : MainViewModel
    {
        private YesContext _db;

        private bool _isLoaded = false;

        private ObservableCollection<AccountViewModel> _accounts;
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts ?? (_accounts = new ObservableCollection<AccountViewModel>());
            set => _accounts = value;
        }

        public AccountsViewModel(YesContext db)
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

                foreach (var a in accounts)
                {
                    var acvm = CreateAccountViewModel(a);
                    Accounts.Add(acvm);
                }

                await Task.Run(async () =>
                {
                    foreach (var acvm in Accounts)
                    {
                        await acvm.InitAsync();
                        await acvm.SaveDataIfNecessary();
                    }
                });

                _isLoaded = true;
            }
        }

        public AccountViewModel CreateAccountViewModel()
        {
            var acvm = ServiceLocator.Current.GetInstance<AccountViewModel>(Guid.NewGuid().ToString());

            acvm.OnSuccessOperation += async o =>
            {
                var acvm2 = (AccountViewModel)o;

                Accounts.Add(acvm2);

                SimpleIoc.Default.Unregister(acvm);
                SimpleIoc.Default.Register(() => acvm2, acvm2.Id.ToString());

                acvm.Removed += AccountRemoved;

                await acvm2.InitAsync();
                await acvm2.SaveData();
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
                Debug.WriteLine("Debug:::: Entered a.Quota == null in CreateAccountViewModel");
                a.Quota = new Quota() { AccountId = a.AccountId };
            }

            return acvm;
        }

        private void AccountRemoved(object source)
        {
            Accounts.Remove(source as AccountViewModel);
        }

        public void RefreshQuota()
        {
            Task.Run(async () =>
            {
                foreach (var acvm in Accounts)
                {
                    await acvm.RefreshDataAsync();
                    await acvm.SaveDataIfNecessary();
                }
            });
        }

    }


}
