using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Windows.Notifications.Toasts
{
    public class QuotaToast : ToastBase
    {
        private const string title = "Logged In";
        private const string content = "Quota Remaining";

        public QuotaToast() : base(title, content)
        {
        }

        protected override void CreateActions()
        {
            _actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Logout", "logout")
                    {
                        ActivationType = ToastActivationType.Background,
                        //ImageUri = "Assets/Reply.png",

                        // Reference the text box's ID in order to
                        // place this button next to the text box
                        //TextBoxId = "tbReply"
                    }
                }
            };
        }
    }
}
