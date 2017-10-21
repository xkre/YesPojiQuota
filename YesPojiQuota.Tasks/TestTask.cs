using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace YesPojiQuota.Tasks
{
    public sealed class TestTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            SendToast("Hi this is background Task");
        }

        //public static void SendToast(string message)
        //{
        //    var template = ToastTemplateType.ToastText01;
        //    var xml = ToastNotificationManager.GetTemplateContent(template);
        //    var elements = xml.GetElementsByTagName("Text");
        //    var text = xml.CreateTextNode(message);

        //    elements[0].AppendChild(text);
        //    var toast = new ToastNotification(xml);
        //    ToastNotificationManager.CreateToastNotifier().Show(toast);
        //}

        public static void SendToast(string message)
        {
            // In a real app, these would be initialized with actual data
            string title = "Andrew sent you a picture";
            string content = message;

            // Construct the visuals of the toast
            ToastVisual visual = new ToastVisual()
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
                            Text = content
                        },

                    },
                }
            };


            // In a real app, these would be initialized with actual data
            int conversationId = 384928;

            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Inputs =
                {
                    new ToastTextBox("tbReply")
                    {
                        PlaceholderContent = "Type a response"
                    }
                },

                Buttons =
                {
                    new ToastButton("Reply","a")
                    {
                        ActivationType = ToastActivationType.Background,
                        TextBoxId = "tbReply"
                    }
                }
            };


            // Now we can construct the final toast content
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = "abc"
            };

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());

            toast.Tag = "18365";
            toast.Group = "wallPosts";

            ToastNotificationManager.CreateToastNotifier().Show(toast);

        }
    }
}
