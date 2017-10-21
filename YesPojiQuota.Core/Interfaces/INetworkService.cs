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
        NetworkCondition NetworkType { get; }

        bool IsConnected();
        Task<NetworkCondition> GetNetworkConditionAsync();
    }
}
