﻿using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace YesPojiQuota.Core.Windows.Notifications.Toasts
{
    public class ToastBase
    {
        public XmlDocument Xml => Content.GetXml();
        public virtual ToastContent Content => _toastContent;

        protected ToastContent _toastContent;

        protected ToastActionsCustom _actions;
        protected ToastVisual _visual;

        public ToastBase(string title, string content)
        {
            CreateContent(title, content);
            CreateActions();
            CreateToast();
        }

        public ToastBase()
        {

        }

        protected virtual void CreateContent(string title, string content)
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
                            Text = content
                        }
                    }
                }
            };
        }

        protected virtual void CreateActions()
        {
            _actions = new ToastActionsCustom();
        }

        private void CreateToast()
        {
            _toastContent = new ToastContent()
            {
                Visual = _visual,
                Actions = _actions
            };
        }
    }
}
