using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace YesPojiQuota.Core.Windows.Managers
{
    public class BackgroundTaskManager
    {
        /*
        public static async Task<bool> RegisterAsync(string name, string entryPoint, IBackgroundTrigger trigger, Action onCompleted = null)
        {
            //var access = await BackgroundExecutionManager.RequestAccessAsync();
            //if (access == BackgroundAccessStatus.DeniedByUser || access == BackgroundAccessStatus.DeniedBySystemPolicy)
            //{
            //    return false;
            //}

            foreach (var t in BackgroundTaskRegistration.AllTasks)
            {
                if (t.Value.Name == name)
                {
                    //t.Value.Unregister(false);
                    return false;
                }
            }

            var builder = new BackgroundTaskBuilder();
            builder.Name = name;
            builder.TaskEntryPoint = entryPoint;
            builder.SetTrigger(trigger);

            var registration = builder.Register();
            if (onCompleted != null)
            {
                registration.Completed += (s, a) =>
                {
                    onCompleted();
                };
            }

            return true;
        }*/

        public static async Task RegisterBackgroundTasks()
        {
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = "LoginToastTask",
                TaskEntryPoint = "YesPojiQuota.Tasks.LoginToastActionBackgroundTask"
            };

            builder.SetTrigger(new ToastNotificationActionTrigger());

            BackgroundTaskRegistration registration = builder.Register();
        }

        /*
        public static async Task RegisterTestTask()
        {
            var task = new BackgroundTaskBuilder
            {
                Name = "My Task",
                TaskEntryPoint = typeof(TestTask).ToString()
            };

            var trigger = new ApplicationTrigger();
            task.SetTrigger(trigger);

            var condition = new SystemCondition(SystemConditionType.InternetNotAvailable);
            task.Register();

            await trigger.RequestAsync();
        }
        */
    }
}