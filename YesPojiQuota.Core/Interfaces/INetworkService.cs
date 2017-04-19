using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Utils.Enums;

namespace YesPojiQuota.Core.Utils.Interfaces
{
    public interface INetworkService
    {
        NetworkCondition NetworkType { get; }

        bool IsConnected();
        void CheckConnection();

        }
    
}
