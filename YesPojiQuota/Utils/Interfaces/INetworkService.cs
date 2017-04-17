using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Utils.Enums;

namespace YesPojiQuota.Utils.Interfaces
{
    public interface INetworkService
    {
        NetworkCondition NetworkType { get; }

        bool IsConnected();
        void CheckConnection();

        }
    
}
