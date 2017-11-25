using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using YesPojiQuota.Core.Observers;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Windows.Utils
{
    internal class WindowsNetworkChangeHandler : NetworkChangeHandler
    {
        public WindowsNetworkChangeHandler(IYesNetworkService ns, IYesSessionService ys) : base(ns, ys)
        {
        }

        protected override void InitNetworkChangeMonitor()
        {
            base.InitNetworkChangeMonitor();

            NetworkInformation.NetworkStatusChanged += CheckNetworkCondition;
            /*
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                var isConnected = (profile != null
                    && profile.GetNetworkConnectivityLevel() ==
                    NetworkConnectivityLevel.InternetAccess);
            };
            */
        }
    }
}
