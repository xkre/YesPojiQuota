﻿using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Utils.Notifications
{
    internal class LoginToast
    {
        public LoginToast()
        {
            // In a real app, these would be initialized with actual data
            string title = "Login to Yes4G Wifi";
            string content = "You are connected to Yes4G network, would you like to login";
            //string image = "https://unsplash.it/360/202?image=883";
            //string logo = "ms-appdata:///local/Andrew.jpg";

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
                        }
                    }

                    //AppLogoOverride = new ToastGenericAppLogo()
                    //{
                    //    Source = logo,
                    //    HintCrop = ToastGenericAppLogoCrop.Circle
                    //}
                }
            };


            // In a real app, these would be initialized with actual data
            int conversationId = 384928;

            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Inputs =
                {
                    new ToastSelectionBox("account")
                    {
                        DefaultSelectionBoxItemId = "ac1",
                        Items =
                        {
                            new ToastSelectionBoxItem("ac1", "AC1"),
                            new ToastSelectionBoxItem("ac2", "AC2"),
                            new ToastSelectionBoxItem("ac3", "AC3")
                        }
                    }
                },

                Buttons =
                {
                    new ToastButton("Login", new QueryString()
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
                    }
                }
            };

        }



    }
}
