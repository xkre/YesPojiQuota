using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Windows.UI.Core;
using YesPojiQuota.Core.Windows.Services;
using YesPojiQuota.Core.Windows.Utils.Messages;

namespace YesPojiQuota.Core.Windows.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        protected INavigationService NavigationService { get; } = SimpleIoc.Default.GetInstance<INavigationService>();

        #region Constructors
        public MainViewModel()
        {
        }
        #endregion Constructors

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async virtual Task InitAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle ?? "Default Title";
            set
            {
                Set("PageTitle", ref _pageTitle, value);
            }
        }

        private string _pageContent;
        public string PageContent
        {
            get { return _pageContent; }
            set { Set("PageContent", ref _pageContent, value); }
        }

        private bool _canLogin = false;
        public bool CanLogin
        {
            get { return _canLogin; }
            set { Set("CanLogin", ref _canLogin, value); }
        }

        //private RelayCommand _sendNotificationMessage;
        //public RelayCommand SendNotificationMessage
        //{
        //    get
        //    {
        //        return _sendNotificationMessage
        //            ?? (_sendNotificationMessage = new RelayCommand(() =>
        //            {
        //                Messenger.Default.Send(
        //                    new NotificationMessageAction<string>(
        //                        "Testing",
        //                        reply =>
        //                        {
        //                            PageTitle = reply;
        //                        }));
        //            }));
        //    }
        //}

        protected void SendNotificationMessage(string message)
        {
            //Messenger.Default.Send(message);
            SendDialogMessage(message, (x) => {; });

            //Messenger.Default.Send(
            //    new NotificationMessageAction<string>(
            //        message,
            //        reply =>
            //        {
            //            //PageTitle = reply;
            //        }
            //    )
            //);
        }

        protected void SendDialogMessage(string message, Action<string> action)
        {
            Messenger.Default.Send(new NotificationMessageAction<string>(message, action));
        }

        protected void SetLoadingMessage(string message)
        {
            Messenger.Default.Send(new LoadingMessage() { IsLoading = true, Message = message });
        }
    }
}
