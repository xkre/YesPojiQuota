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
    public class YesNetworkService : IYesNetworkService
    {
        private const string PORTAL_TEST_URL = "http://detectportal.firefox.com/success.txt";
        private const string SESSION_URL = "https://apc.aptilo.com/apc/session.phtml";

        private IYesSessionService _yss;

        public YesNetworkService()
        {
            _yss = new YesSessionService();
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

        public async Task<NetworkCondition> GetNetworkConditionAsync()
        {
            NetworkCondition condition;

            try
            {
                condition = await IsConnectedToYesAsync() ? NetworkCondition.Online : NetworkCondition.YesWifiConnected;
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

        public async Task<bool> IsConnectedToYesAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(SESSION_URL);
                    var rawHtml = await result.Content.ReadAsStringAsync();

                    var session = _yss.ParseSession(rawHtml);
                    return session.Received > 0 || session.Sent > 0;
                }
                catch
                {
                    throw new YesNotConnectedException("Not Connected to yes network");
                }
            }
        }
    }
}
