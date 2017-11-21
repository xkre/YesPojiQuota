using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace YesPojiQuota.Core.Windows.Notifications
{
    public class Toast
    {
        private const string TaskName = "ToastBackgroundTask";
        private const string TaskEndPoint = "YesPojiQuota.BackgroundTasks.Notifications.ToastBackgroundTask";

        public static async Task RegisterBackgroundTask()
        {
            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if (access == BackgroundAccessStatus.DeniedByUser || access == BackgroundAccessStatus.DeniedBySystemPolicy)
            {
                return;
            }

            //Shameless copy for reference
            //await BackgroundTaskManager.RegisterAsync("NotificationTaskt", "Unigram.Native.Tasks.NotificationTask", new PushNotificationTrigger());
            //await BackgroundTaskManager.RegisterAsync("InteractiveTask", "Unigram.Tasks.InteractiveTask", new ToastNotificationActionTrigger());
        }
        /*
        public static void CreateLoginToast()
        {
            // In a real app, these would be initialized with actual data
            string title = "Andrew sent you a picture";
            string content = "Check this out, Happy Canyon in Utah!";
            string image = "https://unsplash.it/360/202?image=883";
            string logo = "ms-appdata:///local/Andrew.jpg";

            // Construct the visuals of the toast
            ToastVisual toast = new ToastVisual()
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

                        new AdaptiveImage()
                        {
                            Source = image
                        }
                    },

                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = logo,
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    }
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
                    new ToastButton("Reply", new QueryString()
                    {
                        { "action", "reply" },
                        { "conversationId", conversationId.ToString() }

                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Background,
                        ImageUri = "Assets/Reply.png",

                        // Reference the text box's ID in order to
                        // place this button next to the text box
                        TextBoxId = "tbReply"
                    },

                    new ToastButton("Like", new QueryString()
                    {
                        { "action", "like" },
                        { "conversationId", conversationId.ToString() }

                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Background
                    },

                    new ToastButton("View", new QueryString()
                    {
                        { "action", "viewImage" },
                        { "imageUrl", image }

                    }.ToString())
                }
            };
        }*/
    }
}
