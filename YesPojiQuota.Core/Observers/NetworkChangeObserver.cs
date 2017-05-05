using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Enums;

namespace YesPojiQuota.Core.Observers
{
    public class NetworkChangeObserver : IObserver<NetworkCondition>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(NetworkCondition value)
        {
            throw new NotImplementedException();
        }
    }
}
