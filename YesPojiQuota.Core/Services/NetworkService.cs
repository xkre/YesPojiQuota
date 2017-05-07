using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Enums;
using YesPojiQuota.Core.Helpers.Exceptions;
using YesPojiQuota.Core.Interfaces;

namespace YesPojiQuota.Core.Services
{
    public class NetworkService : INetworkService
    {
        private const string PORTAL_TEST_URL = "http://detectportal.firefox.com/success.txt";
        private const string QUOTA_SERVICE_URL = "http://quota.utm.my/";

        public event NetworkChangeEvent NetworkChanged;

        private IDisposable _timer;
        private YesSessionService _yss;

        public NetworkService(YesSessionService ys)
        {
            _yss = ys;
        }

        private NetworkCondition _networkType;
        public NetworkCondition NetworkType
        {
            get => _networkType;
            private set
            {
                _networkType = value;
                NetworkChanged(value);
            }
        }

        /// <summary>
        /// Check for the connection, returns true when connected to yes network. Raises NetworkChangeEvent
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                bool yesConnected = await _yss.IsConnectedToYesAsync();

                NetworkType = yesConnected ? NetworkCondition.Online : NetworkCondition.YesWifiConnected;
                return true;
            }
            catch (YesNotConnectedException yex)
            {
                Debug.WriteLine($"Exception: Yes Network not connected :::: Handled");
                try
                {
                    using (var client = new HttpClient())
                    {
                        await client.GetAsync(QUOTA_SERVICE_URL);
                        NetworkType = NetworkCondition.OnlineNotYes;
                    }
                }
                catch (Exception ex)
                {
                    //When not connected to a network
                    Debug.WriteLine($"Exception {ex}");
                    NetworkType = NetworkCondition.NotConnected;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex}");
            }

            return false;
        }

        private async Task<bool> IsConnectedToYesAsync()
        {
            return await _yss.IsConnectedToYesAsync();
        }

        public void StartMonitor()
        {
            StartMonitor(5, 30);
        }

        public void StartMonitor(int start, int interval)
        {
            var obs = Observable.Timer(TimeSpan.FromMinutes(start), TimeSpan.FromMinutes(interval));

            _timer = obs.Subscribe(x =>
            {
                CheckConnectionAsync();
            });
        }

        public void StopMonitor()
        {
            _timer?.Dispose();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(object source)
        {

        }
    }
}
