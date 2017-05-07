using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Enums;

namespace YesPojiQuota.Core.Observers
{
    public class NetworkChangeObserver : IObserver<NetworkCondition>
    {
        NetworkCondition _currentCondition;
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine($"Exception {error.Message} in NetworkChangeObserver");
        }

        public void OnNext(NetworkCondition value)
        {
            if (value != _currentCondition)
            {

            }
        }
    }
}
