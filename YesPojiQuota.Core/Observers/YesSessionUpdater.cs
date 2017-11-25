﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Services;
using YesPojiUtmLib.Models;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Observers
{
    public class YesSessionUpdater
    {
        public event SessionDataUpdateEvent SessionUpdated;

        //private YesSessionData _session;
        private IDisposable _updateTimer;

        private IYesSessionService _ys;
        private NetworkChangeHandler _nch;

        public YesSessionUpdater(IYesSessionService ys, NetworkChangeHandler nch)
        {
            _ys = ys;
            _nch = nch;
        }

        public void Init()
        {
            _nch.YesConnected += StartMonitor;
            _nch.YesDisconnected += StopMonitor;
        }

        public void StartMonitor()
        {
            var timer = Observable.Timer(TimeSpan.FromMilliseconds(50) ,TimeSpan.FromMinutes(1));

            _updateTimer = timer.Subscribe(ProcessSessionData);
        }

        private async void ProcessSessionData(long x)
        {
            var session = await _ys.GetSessionDataAsync();

            SessionUpdated(session);
        }

        public void StopMonitor()
        {
            _updateTimer?.Dispose();
        }
    }
}
