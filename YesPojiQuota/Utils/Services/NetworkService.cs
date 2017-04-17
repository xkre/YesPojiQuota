using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Utils.Enums;
using YesPojiQuota.Utils.Interfaces;

namespace YesPojiQuota.Utils.Services
{
    public class NetworkService : INetworkService
    {
        public NetworkCondition NetworkType
        {
            get { throw new NotImplementedException(); }
        }

        public void CheckConnection()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe()
        {
            var subscription = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromHours(1));

            return subscription.Subscribe(
                x => Console.WriteLine("Observer 1: OnNext: {0}", x),
                ex => Console.WriteLine("Observer 1: OnError: {0}", ex.Message)
            );
        }
    }
}
