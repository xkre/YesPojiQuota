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
            await RegisterBackgroundTask();

            //await RegisterBackgroundTask2();
            //ShowToast();
        }

        private async Task RegisterBackgroundTask()
        {
            const string taskName = "ToastBackgroundTask";

            // If background task is already registered, do nothing
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(taskName)))
                return;

            // Otherwise request access
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            // Create the background task
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = taskName,
            };

            // Assign the toast action trigger
            builder.SetTrigger(new ToastNotificationActionTrigger());

            // And register the task
            BackgroundTaskRegistration registration = builder.Register();

            //registration.Completed += OnCompleted;
            //registration.Progress += Progress;
        }

        //private async Task RegisterBackgroundTask2()
        //{
        //    const string taskName = "NetworkBackgroundTask";

        //    if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(taskName)))
        //        return;

        //    BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
        //    BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
        //    {
        //        Name = taskName,
        //    };

        //    builder.SetTrigger(new NetworkOperatorHotspotAuthenticationTrigger());
        //    BackgroundTaskRegistration task = builder.Register();

        //    task.Completed += new BackgroundTaskCompletedEventHandler(OnBackgroundTaskCompleted);
        //}

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


        //private void Progress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        //private void OnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        //{
        //    Messenger.Default.Send(new NotificationMessageAction<string>(args.ToString(), (x)=> {; }));
        //}
    }
}
