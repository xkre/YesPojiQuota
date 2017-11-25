using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Windows.Utils;
using YesPojiUtmLib.Models;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Windows.Notifications.Toasts
{
    public class SessionToast : ToastBase
    {
        private YesSessionData _session;
        private IYesSessionService _sessionService;

        public SessionToast() : base()
        {
            _sessionService = AppServiceLocator.YesSessionService;
        }

        protected override void CreateContent(string title, string content)
        {
            _visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title,
                        },
                        new AdaptiveText()
                        {
                            Text = new BindableString("sessionString")
                        }
                    }
                }
            };
        }

        protected override void CreateActions()
        {
            _actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Logout", 
                            new QueryString(){{ "action", "logout" }}.ToString())
                    {
                        ActivationType = ToastActivationType.Background
                    }
                }
            };
        }
    }
}
