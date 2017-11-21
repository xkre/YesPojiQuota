using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace YesPojiQuota.Utils
{
    internal static class BackgroundTaskManager
    {
        private static Dictionary<string, BackgroundTaskRegistration> _taskRegistration;

        static BackgroundTaskManager()
        {
            _taskRegistration = new Dictionary<string, BackgroundTaskRegistration>();
        }

        public static async Task<BackgroundTaskRegistration> RegisterTaskAsync(IBackgroundTrigger trigger, string name, string entryPoint)
        {
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(name)))
                return null;

            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            if (status == BackgroundAccessStatus.DeniedBySystemPolicy || status == BackgroundAccessStatus.DeniedByUser)
                return null;

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = name,
                TaskEntryPoint = entryPoint
            };
            builder.SetTrigger(trigger);

            BackgroundTaskRegistration registration = builder.Register();

            //registration.Completed += OnCompleted;
            //registration.Progress += Progress;
            return registration;
        }

        public static bool UnregisterTask(string name)
        {
            var tasks = BackgroundTaskRegistration.AllTasks.Values.Where(i => i.Name.Equals(name));

            if (tasks.Count() < 1)
                return false;

            foreach (var task in tasks)
            {
                task.Unregister(true);
            }

            return true;
        }

        public static void AttachTaskCompletionEventHandler()
        {
            throw new NotImplementedException();
        }

        public static void AttachTaskProgressEventHandler()
        {
            throw new NotImplementedException();
        }

    }
}
