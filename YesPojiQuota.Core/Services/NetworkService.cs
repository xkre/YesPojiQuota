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
using YesPojiQuota.Core.Interfaces;

namespace YesPojiQuota.Core.Services
{
    public class NetworkService : INetworkService
    {
        private const string PORTAL_TEST_URL = "http://detectportal.firefox.com/success.txt";
        private const string QUOTA_SERVICE_URL = "http://quota.utm.my/";

        public event NetworkChangeEvent NetworkChanged;

        private NetworkCondition _networkType;
        public NetworkCondition NetworkType
        {
            get => _networkType;
            private set
            {
                //if (_networkType != value)
                {
                    _networkType = value;
                    NetworkChanged(value);
                }
            }
        }

        public async Task CheckConnectionAsync()
        {
            string rawHtml = "";
            using (var client = new HttpClient())
            {
                try
                {
                    rawHtml = await client.GetStringAsync(PORTAL_TEST_URL);

                    if (rawHtml.Equals("success\n"))
                    {
                        var response = await client.GetAsync(QUOTA_SERVICE_URL);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            NetworkType = NetworkCondition.Online;
                            return;
                        }

                        NetworkType = NetworkCondition.OnlineNotYes;
                        return;
                    }
                    else
                    {
                        NetworkType = NetworkCondition.YesWifiConnected;
                    }

                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine($"Exception: Connection Error {e.Message}");
                    NetworkType = NetworkCondition.NotConnected;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Exception {e}");
                }

                return;
            }
        }

        //private void RaiseEvent(NetworkCondition condition)
        //{
        //    NetworkChanged?.Invoke(condition);
        //    _networkType = condition;
        //}

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(object source)
        {

        }

        //public IDisposable Subscribe(object source)
        //{

        //}
    }
}
