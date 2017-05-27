using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Observers
{
    public class QuotaObserverManager
    {
        //TODO: Change the subscription strategy...

        private static QuotaObserverManager _instance;
        private Dictionary<Account, Func<Task>> _subscribed;
        private List<Func<Task>> _refreshTasks;

        private NetworkChangeHandler _nch;

        //private bool _yesConnected;
        private IDisposable disposable;

        private QuotaObserverManager(NetworkChangeHandler nch)
        {
            _subscribed = new Dictionary<Account, Func<Task>>();
            _refreshTasks = new List<Func<Task>>();

            _nch = nch;

            _nch.YesConnected += StartMonitor;
            _nch.YesDisconnected += StopMonitor;
        }

        public static QuotaObserverManager Instance
        {
            get => _instance ?? (_instance = new QuotaObserverManager(
                ServiceLocator.Current.GetInstance<NetworkChangeHandler>()));
        }

        public void Subscribe(Account a, Func<Task> b)
        {
            if (_subscribed.ContainsKey(a))
            {
                //_subscribed[a].Dispose();
                _subscribed.Remove(a);
            }

            //var observable = Observable.Interval(TimeSpan.FromMinutes(1));

            //var disp = observable.Subscribe(x => {
            //    if (_yesConnected)
            //        b();
            //    }, ex=>Debug.WriteLine($"Error Refreshing Quota (in observable) {ex.Message}"));

            _subscribed.Add(a, b);
            _refreshTasks.Add(b);
        }

        private void StartMonitor()
        {
            disposable?.Dispose();

            var observable = Observable.Interval(TimeSpan.FromMinutes(5));

            disposable = observable.Subscribe(async x =>
            {
                foreach (var a in _subscribed)
                {
                    await a.Value.Invoke();
                }
            });
        }

        private void StopMonitor()
        {
            disposable?.Dispose();
        }

    }
}
