using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Networking.Connectivity;
using Windows.Networking.NetworkOperators;
using Windows.UI.Notifications;
using YesPojiQuota.Core.Windows.Utils;
using YesPojiQuota.Core.Windows.Notifications.Toasts;
using YesPojiUtmLib.Services;
using System.Diagnostics;

namespace YesPojiQuota.Tasks
{   
    public sealed class NetworkChangeTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private ConnectionProfile _networkStatus;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            _networkStatus = NetworkInformation.GetInternetConnectionProfile();
            var networkLevel = _networkStatus.GetNetworkConnectivityLevel();
            AppServiceLocator appService = new AppServiceLocator();

            try
            {
                if (_networkStatus.IsWlanConnectionProfile)
                {
                    switch (networkLevel)
                    {
                        case NetworkConnectivityLevel.ConstrainedInternetAccess:
                            if (IsConnectedToYesWifi())
                            {
                                ShowLoginToast();
                            }

                            break;
                        case NetworkConnectivityLevel.InternetAccess:
                            if (IsConnectedToYesWifi())
                            {
                                ShowSessionToast();
                            }
                            break;
                    }
                }
            }catch(Exception e)
            {
                ToastHelper.PopToast("Error", e.Message);
            }

            _deferral.Complete();
        }

        private void ShowLoginToast()
        {
            var toastManager = AppServiceLocator.ToastManager;

            //var networkService = new YesNetworkService();
            var accounts = AppServiceLocator.DataService.Accounts;

            var toast = new LoginToast();
            toast.SetAccounts(accounts.ToList());

            Debug.WriteLine(toast.Xml);

            toastManager.ShowToast(toast);
        }

        private void ShowSessionToast()
        {

        }

        private bool IsConnectedToYesWifi()
        {
            return _networkStatus.WlanConnectionProfileDetails.GetConnectedSsid() == "4G WiFi by Yes @ UTM";
        }
    }
}