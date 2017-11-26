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

        public async virtual Task InitAsync()
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

        protected void SendNotificationMessage(string message)
        {
            SendDialogMessage(message, (x) => {; });
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
