using Microsoft.Toolkit.Uwp.Notifications;
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

        protected string _title;
        protected string _content;

        protected ToastContent _toastContent;

        protected ToastActionsCustom _actions;
        protected ToastVisual _visual;

        public ToastBase(string title, string content)
        {
            _title = title;
            _content = content;

            CreateContent();
            CreateActions();

            _toastContent = new ToastContent()
            {
                Visual = _visual,
                Actions = _actions
            };
        }

        protected virtual void CreateContent()
        {
            _visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = _title,
                        },
                        new AdaptiveText()
                        {
                            Text = _content
                        }
                    }
                }
            };
        }

        protected virtual void CreateActions()
        {
            _actions = new ToastActionsCustom();
        }
    }
}
