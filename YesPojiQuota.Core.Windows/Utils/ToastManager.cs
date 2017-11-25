using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Windows.Managers;
using YesPojiQuota.Core.Windows.Notifications.Toasts;

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

            Init();
        }

        public void ShowToast()
        {
            //var loginToast = new LoginToast();
            //loginToast.SetAccounts(_dts.Accounts.ToList());

            //var toast = new ToastNotification(loginToast.Xml);

            //_toastNotier.Show(toast);
        }

        public void ShowToast(ToastBase toast)
        {
            var toastNotification = new ToastNotification(toast.Xml);
            _toastNotier.Show(toastNotification);
        }

        private void Init()
        {
            _toastNotier = ToastNotificationManager.CreateToastNotifier();

            _nch.LoginRequired += ShowToast;
        }

        public async Task InitAsync()
        {
            UnregisterTask();
            await RegisterToastBackgroundTasks();
            //ShowToast();
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
