using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Core.Utils;
using YesPojiQuota.Core.Windows.Services;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Windows.Utils
{
     public class AppServiceLocator : BaseServiceLocator
    {
        public AppServiceLocator() : base()
        {
        }

        protected override void RegisterService()
        {
            base.RegisterService();

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IEncryptionService, WindowsEncryptionService>();
            
            SimpleIoc.Default.Register<NetworkChangeHandler, WindowsNetworkChangeHandler>();

            SimpleIoc.Default.Register<ToastManager>();


            SimpleIoc.Default.Register<IDispatcherHelper, DispatcherHelperEx>();
        }

        public static NetworkChangeHandler NetworkChangeHandler => SimpleIoc.Default.GetInstance<NetworkChangeHandler>();
        public static ToastManager ToastManager => SimpleIoc.Default.GetInstance<ToastManager>();

        public static IEncryptionService EncryptionService => SimpleIoc.Default.GetInstance<IEncryptionService>();
    }
}
