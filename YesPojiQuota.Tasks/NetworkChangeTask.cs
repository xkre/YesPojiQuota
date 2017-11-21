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
using YesPojiQuota.Tasks.Helpers;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Tasks
{   
    public sealed class NetworkChangeTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var networkStatus = NetworkInformation.GetInternetConnectionProfile();

            if (networkStatus.IsWlanConnectionProfile &&
                networkStatus.WlanConnectionProfileDetails.GetConnectedSsid() == "4G WiFi by Yes @ UTM")
            {
                var networkLevel = networkStatus.GetNetworkConnectivityLevel();

                if (networkLevel == NetworkConnectivityLevel.ConstrainedInternetAccess)
                {
                    var networkService = new YesNetworkService();
                    ToastHelper.PopToast("From NetworkChange", "Need to Login???");
                }
            }
            _deferral.Complete();
            // Do the background task activity.
            // First, get the authentication context.

            //var details = taskInstance.TriggerDetails as
            //   HotspotAuthenticationEventDetails;

            //HotspotAuthenticationContext context;
            //if (!HotspotAuthenticationContext.
            //      TryGetAuthenticationContext
            //   (details.EventToken, out context))
            //{
            //    // Event is not of interest. Abort.
            //    return;
            //}

            //byte[] ssid = context.WirelessNetworkId;

            //// Get configuration from application storage.


            //// Check if authentication is handled by foreground app.
            ////if (!Config.AuthenticateThroughBackgroundTask)
            ////{
            ////    // Pass event token to application
            ////    Config.AuthenticationToken = details.EventToken;

            ////    // TriggerAttentionRequired function throws
            ////    // NotImplementedException on phone, we use
            ////    // regular Toast Notification to notify user about
            ////    // the authentication, Tapping on the notification
            ////    // will launch the application where user can
            ////    // complete the authentication.
            ////    var toastXml = ToastNotificationManager.GetTemplateContent
            ////       (ToastTemplateType.ToastText01);
            ////    toastXml.GetElementsByTagName("text")[0].
            ////    AppendChild(toastXml.CreateTextNode("Auth by foreground"));
            ////    IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ////    ((XmlElement)toastNode).SetAttribute("launch",
            ////       "AuthByForeground");

            ////    dynamic toast = new ToastNotification(toastXml);
            ////    Type typeofToastNotification = toast.GetType();
            ////    PropertyInfo tagProperty =
            ////       typeofToastNotification.GetRuntimeProperty("Tag");
            ////    PropertyInfo groupProperty =
            ////       typeofToastNotification.GetRuntimeProperty("Group");
            ////    if (tagProperty != null) toast.Tag = "AuthByForeground";
            ////    if (groupProperty != null) toast.Group = "HotspotAuthAPI";

            ////    var notification =
            ////       ToastNotificationManager.CreateToastNotifier();
            ////    notification.Show(toast);

            ////    return;
            ////}

            //// Handle authentication in background task.

            //// Before calling an asynchronous API from the background task,
            //// get the deferral object from the task instance.
            //_deferral = taskInstance.GetDeferral();

            //// Finally, call SkipAuthentication to indicate that we
            //// are not doing native WISPr authentication.
            //// This call also serves the purpose of indicating a
            //// successful authentication.
            //context.SkipAuthentication();

            //_deferral.Complete();
        }
    }
}

