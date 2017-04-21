using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Enums;

namespace YesPojiQuota.Core.Interfaces
{
    public interface INetworkService
    {
        event NetworkChangeEvent NetworkChanged;
        NetworkCondition NetworkType { get; }

        bool IsConnected();
        Task CheckConnectionAsync();

        void StartMonitor();
        void StopMonitor();
    }

    public delegate void NetworkChangeEvent(NetworkCondition condition);
    
}
