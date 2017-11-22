using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Windows.Notifications.Toasts
{
    internal class LoginToast : ToastBase
    {
        const string title = "Login to Yes4G Wifi";
        const string content = "You are connected to Yes4G network, would you like to login?";

        public LoginToast() : base (title, content)
        {
        }

        protected override void CreateActions()
        {
            _actions = new ToastActionsCustom()
            {
                Inputs =
                {
                    new ToastSelectionBox("account")
                },

                Buttons =
                {
                    new ToastButton("Login", new QueryString()
                    {
                        { "action", "login" }
                    }.ToString())
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

        public void SetAccounts(IList<Account> accounts)
        {
            if (accounts.Count > 0)
            {
                if (accounts.Count > 5)
                    accounts = accounts.Where(x=> x.Password.Length > 0)
                                    .OrderBy(x=> x.Quota.Available).Take(5).ToList();

                var toastSelectionBox = _actions.Inputs[0] as ToastSelectionBox;

                foreach (var account in accounts)
                {
                    toastSelectionBox.Items.Add(new ToastSelectionBoxItem(account.Username, account.Username));
                }

                toastSelectionBox.DefaultSelectionBoxItemId = accounts[0].Username;
            }
            return;
        }
    }
}
