﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Utils;
using YesPojiUtmLib.Enums;
using YesPojiUtmLib.Models;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.ViewModels
{
    public class InnAppToastViewModel : MainViewModel
    {
        private IYesNetworkService _ns;
        private IYesLoginService _ls;
        private YesSessionUpdater _ys;
        private NetworkChangeHandler _nch;

        private ToastManager _tm;

        private IDisposable _messageTimer;

        public InnAppToastViewModel(IYesNetworkService ns, IYesLoginService ls,
            YesSessionUpdater ys, NetworkChangeHandler nch, ToastManager tm)
        {
            _ns = ns;
            _ls = ls;
            _ys = ys;
            _nch = nch;
            _tm = tm;
        }

        #region Properties
        private bool _visiblilty = true;
        public bool Visibility
        {
            get { return _visiblilty; }
            set { Set("Visibility", ref _visiblilty, value); }
        }

        private string _status;
        public string Status
        {
            get => _status; 
            set => Set("Status", ref _status, value); 
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

        private string _received = "";
        public string Received
        {
            get { return _received; }
            set { Set("Received", ref _received, value); }
        }

        private string _sent = "";
        public string Sent
        {
            get { return _sent; }
            set { Set("Sent", ref _sent, value); }
        }

        private string _timeConnected = "";
        public string TimeConnected
        {
            get { return _timeConnected; }
            set { Set("TimeConnected", ref _timeConnected, value); }
        }
        #endregion Properties

        public override async Task InitAsync()
        {
            await base.InitAsync();

            await InitLoading();

            _nch.NetworkChanged += UpdateNetworkStatusDisplay;
            _ys.SessionUpdated += ProcessSessionUpdate;

            await _tm.InitAsync();

            //_ls.OnLoginFailed += ProcessLoginFail;
            //_ls.OnLoginSuccess += ProcessLoginSuccess;

            InitMessengerRegistration();
        }

        public async void Logout()
        {
            SetLoadingMessage("Logging out");
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

            IsLoading = false;
        }

        private async Task InitLoading()
        {
            await DispatcherHelper.RunAsync(() =>
            {
                Message = "Checking network status";
                IsLoading = true;
            });
        }

        private void InitMessengerRegistration()
        {
            Messenger.Default.Register<string>(this, HandleNotification);
            //TODO: Temporary   -- Seriously -------------------------------------
            Messenger.Default.Register<bool>(this, HandleNotification);
            Messenger.Default.Register<LoadingMessage>(this, async (message) =>
            {
                _messageTimer?.Dispose();

                await DispatcherHelper.RunAsync(() =>
                {
                    Message = message.Message;
                    IsLoading = message.IsLoading;
                });

                _messageTimer = Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(async (x) =>
                {
                    await DispatcherHelper.RunAsync(() =>
                    {
                        Message = "";
                        IsLoading = false;
                    });
                });
            });

            Messenger.Default.Register<LoginMessage>(this, HandleLoginMessage);
            ////Temporary   -- Seriously -------------------------------------
        }

        private void ProcessSessionUpdate(YesSessionData data)
        {
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DispatcherHelper.RunAsync(() =>
            {
                 Received      = $"{(data.Received / 1024):N3} MB";
                 Sent          = $"{(data.Sent     / 1024):N3} MB";
                 TimeConnected = data.Time.Hours > 0 ? $"{data.Time.Hours} Hours {data.Time.Minutes:D2} Minutes" 
                                                     : $"{data.Time.Minutes:D2} Minutes";
            });
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void HandleNotification(string a)
        {
            DispatcherHelper.RunAsync(() => Message = a);
        }

        private void HandleNotification(bool a)
        {
            DispatcherHelper.RunAsync(() => IsConnected = a);
        }

        private async void UpdateNetworkStatusDisplay(NetworkCondition condition)
        {
            await DispatcherHelper.RunAsync(() =>
            {
                Message = "";
                IsConnected = false;
                switch (condition)
                {
                    case NetworkCondition.Online:
                        Status = "Connected";
                        IsConnected = true;
                        break;
                    case NetworkCondition.OnlineNotYes:
                        Status = "Online, not on Yes Network";
                        break;
                    case NetworkCondition.NotConnected:
                        Status = "Not online";
                        break;
                    case NetworkCondition.YesWifiConnected:
                        Status = "Login Required";
                        break;
                }

                IsLoading = false;
            });
        }

        private async void HandleLoginMessage(LoginMessage message)
        {
            //TODO: Maybe separate this into a class
            if (message.Status == LoginStatus.Success)
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    Message = "Login Success";
                    IsConnected = true;
                    IsLoading = false;
                });
            }
            else
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    Message = $"Login Not Successful - {message.ToString()}";
                    IsLoading = false;
                });
            }
        }
        /*
        private async void ProcessLoginSuccess()
        {
            //TODO: Show session data
            await DispatcherHelper.RunAsync(() =>
            {
                Message = "Login Success";
                IsConnected = true;
                IsLoading = false;
            });
        }

        private async void ProcessLoginFail(LoginFailureReason reason)
        {
            //TODO: Show Login fail message
            await DispatcherHelper.RunAsync(() =>
            {
                Message = $"Login Not Successful - {reason.ToString()}";
                IsLoading = false;
            });
        }
        */
    }
}
