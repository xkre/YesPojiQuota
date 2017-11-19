using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Services;
using Windows.Networking.Connectivity;
using YesPojiQuota.Core.Helpers;
using YesPojiUtmLib.Services;
using YesPojiQuota.Core.Interfaces;
using YesPojiUtmLib.Enums;

namespace YesPojiQuota.Core.Observers
{
    public class NetworkChangeHandler
    {
        public event NetworkChangeEvent NetworkChanged;
        public event SimpleEvent YesConnected;
        public event SimpleEvent YesDisconnected;
        public event SimpleEvent WentOnline;
        public event SimpleEvent WentOffline;

        private IYesNetworkService _ns;
        private YesSessionService _ys;

        private NetworkCondition _currentNetwork;
        private IDisposable _networkChangeSubscription;
        private bool _yesConnected;
        private bool _isInitialized;

        public NetworkChangeHandler(IYesNetworkService ns, YesSessionService ys)
        {
            _ns = ns;
            _ys = ys;

            _currentNetwork = NetworkCondition.Undetermined;
        }

        public NetworkCondition CurrentNetwork => _currentNetwork;

        public void Init()
        {
            StartNetworkMonitor();
            InitNetworkChangeMonitor();

            Debug.WriteLine("Initialized NetworkChangeHandler");
        }

        private void ProcessSessionUpdate(SessionData data)
        {
            throw new NotImplementedException();
        }

        private void ProcessNetworkChange(NetworkCondition condition)
        {
            if (_currentNetwork != condition)
            {
                NetworkChanged(condition);
                _currentNetwork = condition;

                if (condition == NetworkCondition.Online ||
                    condition == NetworkCondition.YesWifiConnected)
                {
                    if (!_isInitialized || !_yesConnected)
                    {
                        _yesConnected = true;

                        YesConnected();
                        Debug.WriteLine("NetworkChangeHandler Raised YesConnected Event");
                    }
                }
                else if (condition == NetworkCondition.NotConnected ||
                         condition == NetworkCondition.OnlineNotYes)
                {
                    if (!_isInitialized || _yesConnected)
                    {
                        _yesConnected = false;

                        YesDisconnected();
                        Debug.WriteLine("NetworkChangeHandler Raised YesDisconnected Event");
                    }
                }

                _isInitialized = true;
            }
        }

        /// <summary>
        /// Network monitor by timer
        /// </summary>
        private void StartNetworkMonitor()
        {
            var obs = Observable.Timer(TimeSpan.FromMilliseconds(10),
                                       TimeSpan.FromMinutes(5));

            _networkChangeSubscription = obs.Subscribe(CheckNetworkCondition);
        }

        private void CheckNetworkCondition(long s) => CheckNetworkCondition((object)s);

        private async void CheckNetworkCondition (object s)
        {
            var network = await _ns.GetNetworkConditionAsync();
            ProcessNetworkChange(network);
        }

        private void InitNetworkChangeMonitor()
        {
            NetworkInformation.NetworkStatusChanged += CheckNetworkCondition;
                //{
                //    //var profile = NetworkInformation.GetInternetConnectionProfile();
                //    //var isConnected = (profile != null
                //    //    && profile.GetNetworkConnectivityLevel() ==
                //    //    NetworkConnectivityLevel.InternetAccess);
            //};
        }
    }
}