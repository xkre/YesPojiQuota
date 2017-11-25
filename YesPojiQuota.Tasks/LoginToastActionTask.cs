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
using YesPojiQuota.Core.Interfaces;

namespace YesPojiQuota.Tasks
{
    public sealed class LoginToastActionTask : IBackgroundTask
    {
        private IDataService _data;
        private IEncryptionService _es;
        private IYesLoginService _ls;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            AppServiceLocator appService = new AppServiceLocator();

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;

            //ToastHelper.PopToast("DEBUG: toast argument", details.Argument
            if (details != null)
            {
                switch (details.Argument)
                {
                    case "login":
                        await Login(details);
                        break;

                    case "logout":
                        await Logout();
                        break;
                }
            }

            deferral.Complete();
        }

        private async Task Login(ToastNotificationActionTriggerDetail details)
        {
            _data = AppServiceLocator.DataService;
            _es = AppServiceLocator.EncryptionService;
            _ls = AppServiceLocator.YesLoginService;

            var input = details.UserInput.Values.FirstOrDefault().ToString();
            var account = _data.Accounts.Where(x => x.Username == input).FirstOrDefault();

            account.Password = _es.AES_Decrypt(account.Password, account.Username);

            var status = await _ls.LoginAsync(account);
            ToastHelper.PopToast("LOGIN Status:", status.ToString());
        }

        private async Task Logout()
        {
            _ls = AppServiceLocator.YesLoginService;

            await _ls.LogoutAsync();
        }
    }
}