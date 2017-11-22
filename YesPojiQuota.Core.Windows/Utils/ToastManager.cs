using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Core.Windows.Managers;
using YesPojiQuota.Core.Windows.Notifications.Toasts;
using YesPojiQuota.Core.Windows.Utils;

namespace YesPojiQuota.Core.Windows.Utils
{
    public class ToastManager
    {
        private ToastNotifier _toastNotier;
        private NetworkChangeHandler _nch;
        private IDataService _dts;

        public ToastManager(NetworkChangeHandler nch, IDataService dts)
        {
            _nch = nch;
            _dts = dts;
        }

        public void ShowToast()
        {
            _toastNotier = ToastNotificationManager.CreateToastNotifier();

            var loginToast = new LoginToast();
            loginToast.SetAccounts(_dts.Accounts.ToList());

            var toast = new ToastNotification(loginToast.Xml);

            _toastNotier.Show(toast);
        }

        public async Task InitAsync()
        {
            _nch.LoginRequired += ShowToast;
            UnregisterTask();

            await RegisterToastBackgroundTasks();
            ShowToast();
        }

        private async Task RegisterToastBackgroundTasks()
        {
            await BackgroundTaskManager.RegisterTaskAsync(new ToastNotificationActionTrigger(), "LoginToastActionTask", "YesPojiQuota.Tasks.LoginToastActionTask");
            await BackgroundTaskManager.RegisterTaskAsync(new SystemTrigger(SystemTriggerType.NetworkStateChange, false), "NetworkChangeTask", "YesPojiQuota.Tasks.NetworkChangeTask");

            //task.Completed += new BackgroundTaskCompletedEventHandler(OnBackgroundTaskCompleted);
        }

        private void UnregisterTask()
        {
            var tasks = BackgroundTaskRegistration.AllTasks.Values;

            foreach (var task in tasks)
            {
                if (task != null)
                    task.Unregister(true);
            }
        }

        private void OnBackgroundTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            ToastHelper.PopToast("BackgroundCompleted", "NetworkBackgroundTask Completed");
        }
    }
}
