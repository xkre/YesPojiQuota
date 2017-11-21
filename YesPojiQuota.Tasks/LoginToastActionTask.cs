using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Windows;
using YesPojiQuota.Core.Helpers;
using YesPojiUtmLib.Services;
using YesPojiQuota.Core.Windows.Services;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Core.Windows.Utils;

namespace YesPojiQuota.Tasks
{
    public sealed class LoginToastActionTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            ToastHelper.PopToast("LOGIN", "debug");

            var deferral = taskInstance.GetDeferral();

            YesContext _db = new YesContext();
            DataService _data = new DataService(_db);
            WindowsEncryptionService _es = new WindowsEncryptionService();

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            var input = details.UserInput.Values.FirstOrDefault().ToString();

            var _ls = new YesLoginService();
            var account = _data.Accounts.Where(x => x.Username == input).FirstOrDefault();

            account.Password = _es.AES_Decrypt(account.Password, account.Username);

            var status = await _ls.LoginAsync(account);
            ToastHelper.PopToast("LOGIN", status.ToString());

            /*
            var details = args.taskinstance.triggerdetails as toastnotificationactiontriggerdetail;
            if (details != null)
            {
                string arguments = details.argument;
                var userinput = details.userinput;
                // perform tasks
            }
            */
            deferral.Complete();
        }
    }
}
