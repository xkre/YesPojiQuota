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
        Task<bool> CheckConnectionAsync();

        void StartMonitor();
        void StopMonitor();
    }
}
