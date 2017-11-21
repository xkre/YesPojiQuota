using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Helpers;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Tasks.Helpers;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Tasks
{
    public sealed class LoginToastActionTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            if (taskInstance.Task.Name == "ToastBackgroundTask")
            {
                YesContext _db = new YesContext();
                DataService _data = new DataService(_db);

                var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
                var input = details.UserInput.Values.FirstOrDefault().ToString();

                var _ls = new YesLoginService();
                var account = _data.Accounts.Where(x => x.Username == input).FirstOrDefault();

                account.Password = EncryptionHelper.AES_Decrypt(account.Password, account.Username);

                var status = await _ls.LoginAsync(account);
                ToastHelper.PopToast("LOGIN", status.ToString());
            }

            //var details = args.TaskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            //if (details != null)
            //{
            //    string arguments = details.Argument;
            //    var userInput = details.UserInput;

            //    // Perform tasks

            //}
            deferral.Complete();
        }
    }
}
