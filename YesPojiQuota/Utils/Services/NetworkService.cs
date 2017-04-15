using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}
