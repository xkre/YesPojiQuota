using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Core.Windows.Services;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Windows.Utils
{
     public class AppServiceLocator
    {
        public AppServiceLocator()
        {
            RegisterService();
        }

        private void RegisterService()
        {
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            SimpleIoc.Default.Register<IYesLoginService, YesLoginService>();
            SimpleIoc.Default.Register<IYesQuotaService, YesQuotaService>();
            SimpleIoc.Default.Register<IYesNetworkService, YesNetworkService>();
            SimpleIoc.Default.Register<IYesSessionService, YesSessionService>();

            SimpleIoc.Default.Register<IEncryptionService, WindowsEncryptionService>();
            SimpleIoc.Default.Register<IDataService, DataService>();
            
            SimpleIoc.Default.Register<NetworkChangeHandler, WindowsNetworkChangeHandler>();
            SimpleIoc.Default.Register<QuotaObserverManager>();
            SimpleIoc.Default.Register<YesSessionUpdater>();
            SimpleIoc.Default.Register<ToastManager>();

            SimpleIoc.Default.Register<YesContext>();
        }

        public static IYesLoginService YesLoginService => SimpleIoc.Default.GetInstance<IYesLoginService>();
        public static IYesQuotaService YesQuotaService => SimpleIoc.Default.GetInstance<IYesQuotaService>();
        public static IYesNetworkService YesNetworkService => SimpleIoc.Default.GetInstance<IYesNetworkService>();
        public static IYesSessionService YesSessionService => SimpleIoc.Default.GetInstance<IYesSessionService>();

        public static NetworkChangeHandler NetworkChangeHandler => SimpleIoc.Default.GetInstance<NetworkChangeHandler>();
        public static QuotaObserverManager QuotaObserverManager => SimpleIoc.Default.GetInstance<QuotaObserverManager>();
        public static ToastManager ToastManager => SimpleIoc.Default.GetInstance<ToastManager>();

        public static IEncryptionService EncryptionService => SimpleIoc.Default.GetInstance<IEncryptionService>();
        public static IDataService DataService => SimpleIoc.Default.GetInstance<IDataService>();

        public static YesContext YesContext => SimpleIoc.Default.GetInstance<YesContext>();
        public static YesSessionUpdater YesSessionUpdater => SimpleIoc.Default.GetInstance<YesSessionUpdater>();
    }
}
