using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace YesPojiQuota.Core.Windows.Managers
{
    public static class BackgroundTaskManager
    {
        public static async Task RegisterTaskAsync(IBackgroundTrigger trigger, string name, string entryPoint)
        {
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(name)))
                return;

            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            if (status == BackgroundAccessStatus.DeniedBySystemPolicy || status == BackgroundAccessStatus.DeniedByUser)
                return;

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = name,
                TaskEntryPoint = entryPoint
            };
            builder.SetTrigger(trigger);

            Debug.WriteLine($"Registering BG Task : {name}");
            BackgroundTaskRegistration registration = builder.Register();

            //registration.Completed += OnCompleted;
            //registration.Progress += Progress;
            return;
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