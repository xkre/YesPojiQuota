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
        public async virtual Task Init()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

        }


        protected readonly INavigationService _navigationService;

        private string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle ?? "Default Title"; }
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


        //private RelayCommand _navigateToSecondPage;
        //public RelayCommand NavigateToSecondPage
        //{
        //    get
        //    {
        //        return _navigateToSecondPage ?? (_navigateToSecondPage = new RelayCommand(() => _navigationService.NavigateTo(ViewModelKeys.SECOND_PAGE)));
        //    }
        //}

        private bool _canLogin = false;
        public bool CanLogin
        {
            get { return _canLogin; }
            set { Set("CanLogin", ref _canLogin, value); }
        }

        //private RelayCommand _navigateToAddZone;

        //public RelayCommand NavigateToAddZone
        //{
        //    get
        //    {
        //        return _navigateToAddZone ?? (_navigateToAddZone = new RelayCommand(() => _navigationService.NavigateTo(ViewModelKeys.ADD_ZONE_PAGE)));
        //    }
        //}

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



        //private RelayCommand _showWarning;

        //public RelayCommand ShowWarning
        //{
        //    get
        //    {
        //        return _showWarning
        //            ?? (_showWarning = new RelayCommand(
        //                async () =>
        //                {
        //                    var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
        //                    await dialog.ShowMessage("This is a warning.", "Warning");
        //                }));
        //    }
        //}

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
