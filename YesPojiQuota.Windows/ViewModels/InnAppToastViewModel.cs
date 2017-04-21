using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Enums;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Services;

namespace YesPojiQuota.ViewModels
{
    public class InnAppToastViewModel : MainViewModel
    {
        private INetworkService _ns;
        private ILoginService _ls;

        public InnAppToastViewModel(INetworkService ns, ILoginService ls)
        {
            _ns = ns;
            _ls = ls;
        }

        #region Properties
        private bool _visiblilty;
        public bool Visibility
        {
            get { return _visiblilty; }
            set { Set("Visibility", ref _visiblilty, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { Set("Message", ref _message, value); }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set { Set("IsConnected", ref _isConnected, value); }
        }

        #endregion Properties

        public override async Task InitAsync()
        {
            await base.InitAsync();

            _ns.NetworkChanged += ProcessNetworkNotification;
            await Task.Run(() =>
            {
                _ns.CheckConnectionAsync();
                _ns.StartMonitor();
            });

            Messenger.Default.Register<string>(this, HandleNotification);
            //TODO: Temporary   -- Seriously -------------------------------------
            Messenger.Default.Register<bool>(this, HandleNotification);
            //Temporary   -- Seriously -------------------------------------
        }

        private void HandleNotification(string a)
        {
            Message = a;
        }

        private void HandleNotification(bool a)
        {
            IsConnected = a;
        }

        public async void Logout()
        {
            var success = await _ls.LogoutAsync();

            if (success)
            {
                Message = "Logout Successfull";
            }

            IsConnected = false;
        }

        //private RelayCommand _logoutCommand;
        //public RelayCommand LogoutCommand
        //{
        //    get
        //    {
        //        return _logoutCommand ?? (_logoutCommand = new RelayCommand(
        //                () => Logout(), 
        //                () => IsConnected)
        //            );
        //    }
        //}


        public async void ProcessNetworkNotification(NetworkCondition condition)
        {
            await DispatcherHelper.RunAsync(()=>
            {
                IsConnected = false;
                switch (condition)
                {
                    case NetworkCondition.NotConnected:
                        Message = "Not online";
                        break;
                    case NetworkCondition.Online:
                        Message = "Connected";
                        IsConnected = true;
                        break;
                    case NetworkCondition.YesWifiConnected:
                        Message = "Login Required";
                        break;
                    case NetworkCondition.OnlineNotYes:
                        Message = "Online, not on Yes Network";
                        break;
                }

                Visibility = true;
            });
        }
    }
}
