using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Utils;
using Windows.UI.Core;
using System.Data;
using GalaSoft.MvvmLight.Ioc;
using YesPojiQuota.Core.Interfaces;
using YesPojiUtmLib.Enums;

namespace YesPojiQuota.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructors
        [PreferredConstructor]
        public MainViewModel(INavigationService navigationService) : this()
        {
            _navigationService = navigationService;
        }

        public MainViewModel()
        {
            PageTitle = "Main Page";
        }
        #endregion Constructors

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async virtual Task InitAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

        protected readonly INavigationService _navigationService;

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

        internal void BackRequested(object sender, BackRequestedEventArgs e)
        {
            var nav = (ICustomNavigationService)_navigationService;

            if (nav.GoBackSystem())
            {
                e.Handled = true;
            }
        }
    }
}
