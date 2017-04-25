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
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Services;

namespace YesPojiQuota.ViewModels
{
    public class InnAppToastViewModel : MainViewModel
    {
        private INetworkService _ns;
        private ILoginService _ls;
        private YesSessionService _ys;

        public InnAppToastViewModel(INetworkService ns, ILoginService ls, YesSessionService ys)
        {
            _ns = ns;
            _ls = ls;
            _ys = ys;
        }

        #region Properties
        private bool _visiblilty = true;
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

        private bool _isLoading = true;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set("IsLoading", ref _isLoading, value); }
        }


        #endregion Properties

        public override async Task InitAsync()
        {
            await base.InitAsync();

            _ns.NetworkChanged += ProcessNetworkNotification;
            _ys.SessionUpdated += ProcessSessionUpdate;

            Message = "Checking network status";

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

        public void InitLoading()
        {
            Message = "Checking network status";
            IsLoading = true;
        }

        private void ProcessSessionUpdate(SessionData data)
        {
            //TODO TODO -----------------------------------------------------------
            throw new NotImplementedException();
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
                IsConnected = false;
            }
            else
            {
                Message = "Logout Not Successfull";
            }
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
                    case NetworkCondition.Online:
                        Message = "Connected";
                        IsConnected = true;
                        break;
                    case NetworkCondition.OnlineNotYes:
                        Message = "Online, not on Yes Network";
                        break;
                    case NetworkCondition.NotConnected:
                        Message = "Not online";
                        break;
                    case NetworkCondition.YesWifiConnected:
                        Message = "Login Required";
                        break;
                }

                IsLoading = false;
            });
        }
    }
}
