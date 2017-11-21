using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Utils.Notifications;

namespace YesPojiQuota.Utils
{
    public class ToastManager
    {
        private ToastNotifier _toastNotier;
        private NetworkChangeHandler _nch;


        public ToastManager(NetworkChangeHandler nch)
        {
            _nch = nch;
        }

        public void ShowToast()
        {
            _toastNotier = ToastNotificationManager.CreateToastNotifier();

            var loginToast = new LoginToast();
            var toast = new ToastNotification(loginToast.Content.GetXml());

            _toastNotier.Show(toast);
        }

        public async Task InitAsync()
        {
            _nch.LoginRequired += ShowToast;
            UnregisterTask();

            await RegisterToastBackgroundTasks();
            //ShowToast();
        }

        private async Task RegisterToastBackgroundTasks()
        {
            await BackgroundTaskManager.RegisterTaskAsync(new ToastNotificationActionTrigger(), "ToastBackgroundTask", "YesPojiQuota.Tasks.LoginToastActionTask");
            await BackgroundTaskManager.RegisterTaskAsync(new ToastNotificationActionTrigger(), "NetworkBackgroundTask", "YesPojiQuota.Tasks.NetworkChangeTask");

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
