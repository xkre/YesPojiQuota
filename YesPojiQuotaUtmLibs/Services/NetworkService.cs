using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Enums;
using YesPojiQuotaUtmLibs.Exceptions;

namespace YesPojiQuotaUtmLibs.Services
{
    public class NetworkService
    {
        private YesSessionService _yss;
        private const string PORTAL_TEST_URL = "";

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
            }
        }

        /// <summary>
        /// Check for the connection, returns true when connected to yes network. Raises NetworkChangeEvent
        /// </summary>
        /// <returns></returns>
        /*
        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                NetworkType = await _yss.IsConnectedToYesAsync() ? NetworkCondition.Online : NetworkCondition.YesWifiConnected;
                return true;
            }
            catch (YesNotConnectedException yex)
            {
                Debug.WriteLine($"Exception: Yes Network not connected :::: Handled");
                try
                {
                    using (var client = new HttpClient())
                    {
                        await client.GetAsync(PORTAL_TEST_URL);
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
        */

        public async Task<NetworkCondition> GetNetworkConditionAsync()
        {
            NetworkCondition condition;

            try
            {
                condition = await _yss.IsConnectedToYesAsync() ? NetworkCondition.Online : NetworkCondition.YesWifiConnected;
            }
            catch (YesNotConnectedException)
            {
                Debug.WriteLine($"Exception: Yes Network not connected :::: Handled");
                try
                {
                    using (var client = new HttpClient())
                    {
                        await client.GetAsync(PORTAL_TEST_URL);
                        condition = NetworkCondition.OnlineNotYes;
                    }
                }
                catch (Exception ex)
                {
                    //When not connected to a network
                    Debug.WriteLine($"Exception {ex.Message}");
                    condition = NetworkCondition.NotConnected;
                }
            }
            catch (Exception ex)
            {
                //In case of the unexpected
                Debug.WriteLine($"Exception: {ex.Message}");
                condition = NetworkCondition.NotConnected;
            }

            return condition;
        }

        private async Task<bool> IsConnectedToYesAsync()
        {
            return await _yss.IsConnectedToYesAsync();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}
