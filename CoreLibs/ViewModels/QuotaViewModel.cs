using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.CoreLibs.Data;
using YesPojiQuota.CoreLibs.Enums;
using YesPojiQuota.CoreLibs.Interfaces;
using YesPojiQuota.CoreLibs.Models;
using YesPojiQuota.CoreLibs.Observers;
using YesPojiQuota.CoreLibs.Services;

namespace YesPojiQuota.CoreLibs.ViewModels
{
    public class QuotaViewModel : MainViewModel
    {
        private YesContext _db;
        private IQuotaService _qs;
        private QuotaObserverManager _qom;

        private bool _isChanged = false;
        private IDisposable quotaObserver;

        public QuotaViewModel(YesContext db, IQuotaService qs, QuotaObserverManager qom)
        {
            _db = db;
            _qs = qs;
            _qom = qom;
        }

        private Quota _quota;
        public Quota Quota
        {
            get => _quota;
            set
            {
                Set("Quota", ref _quota, value);
                Available = value.Available;
            }
        }

        private double _available;
        public double Available
        {
            get => _available;
            set
            {
                Set("Available", ref _available, value);
                RaisePropertyChanged("QuotaString");
            }
        }

        public double MaxQuota => _qs.GetMaxQuota(_quota.Account.Username);

        public string QuotaString
        {
            get => String.Format($"{Available:0.###} MB Available");
        }

        private bool IsChanged { get; set; }

        public void QueueSaveQuota(double available)
        {
            if (IsChanged)
            {
                IsChanged = false;
            }
        }

        public override async Task InitAsync()
        {
            SubscribeToQuotaChange();
        }

        public async Task SaveDataIfNecessary()
        {
            if (_isChanged)
            {
                await SaveData();
            }
        }

        public async Task SaveData()
        {
            Debug.WriteLine($"Saving quota data for: {Quota.Account.Username}");

            var quota = _db.Quotas.Where(x => x.AccountId == _quota.AccountId).FirstOrDefault();
            if (quota != null)
            {
                quota.Available = Available;
                await _db.SaveChangesAsync();
            }
        }

        public async Task RefreshQuotaAsync()
        {
            Debug.WriteLine($"Refreshing quota for: {Quota.Account.Username}");
            try
            {
                var available = await _qs.GetQuota(Quota.Account.Username);

                _isChanged = available != Available;

                Quota.Available = Available;
                await DispatcherHelper.RunAsync(() => Available = available);
            }
            catch
            {
                Debug.WriteLine($"Unable to refresh quota");
            }
        }

        private void SubscribeToQuotaChange()
        {
            _qom.Subscribe(Quota.Account, async () => 
            {
                //Debug.WriteLine($"Attempting to refresh Quota for Account: {Quota.Account.Username}");
                await RefreshQuotaAsync();
                await SaveDataIfNecessary();
            });
        }
    }
}
