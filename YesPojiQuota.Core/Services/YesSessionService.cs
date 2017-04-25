using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Interfaces;

namespace YesPojiQuota.Core.Services
{
    public class YesSessionService
    {
        public event SessionDataUpdateEvent SessionUpdated;

        private SessionData _session;
        private IDisposable _timer;


        private void Update()
        {
            //DO something here




            SessionUpdated(_session);
        }

        public void StartMonitor()
        {
            var timer = Observable.Timer(TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(3));

            _timer = timer.Subscribe(
                (x) => Update()
                );
        }

        public void StopMonitor()
        {
            _timer?.Dispose();
        }

    }
}
