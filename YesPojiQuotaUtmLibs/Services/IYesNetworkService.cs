using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Enums;

namespace YesPojiQuotaUtmLibs.Services
{
    interface IYesNetworkService
    {
        Task<NetworkCondition> GetNetworkConditionAsync();
        Task<bool> IsConnectedToYesAsync();
    }
}
