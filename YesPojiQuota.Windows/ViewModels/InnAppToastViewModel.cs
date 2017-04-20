using GalaSoft.MvvmLight.Messaging;
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

        public InnAppToastViewModel(INetworkService ns)
        {
            _ns = ns;
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
        #endregion Properties



        public override async Task InitAsync()
        {
            await base.InitAsync();

            _ns.NetworkChanged += ProcessNotification;
            await _ns.CheckConnectionAsync();

            Messenger.Default.Register<string>(this, HandleNotification);

        }

        private async void HandleNotification(string a)
        {
            Message = a;
        }


        public void ProcessNotification(NetworkCondition condition)
        {
            switch (condition)
            {
                case NetworkCondition.NotConnected:
                    Message = "Not online";
                    break;
                case NetworkCondition.Online:
                    Message = "Connected";
                    break;
                case NetworkCondition.YesWifiConnected:
                    Message = "Login Required";
                    break;
                case NetworkCondition.OnlineNotYes:
                    Message = "Not on Yes Network";
                    break;
            }

            Visibility = true;
        }
    }
}
