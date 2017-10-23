using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.CoreLibs.Enums;

namespace YesPojiQuota.CoreLibs.Interfaces
{
    public interface INetworkService
    {
        NetworkCondition NetworkType { get; }

        bool IsConnected();
        Task<NetworkCondition> GetNetworkConditionAsync();
    }
}
