using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
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
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

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

        public static IYesLoginService YesLoginService => ServiceLocator.Current.GetInstance<IYesLoginService>();
        public static IYesQuotaService YesQuotaService => ServiceLocator.Current.GetInstance<IYesQuotaService>();
        public static IYesNetworkService YesNetworkService => ServiceLocator.Current.GetInstance<IYesNetworkService>();
        public static IYesSessionService YesSessionService => ServiceLocator.Current.GetInstance<IYesSessionService>();

        public static NetworkChangeHandler NetworkChangeHandler => ServiceLocator.Current.GetInstance<NetworkChangeHandler>();
        public static QuotaObserverManager QuotaObserverManager => ServiceLocator.Current.GetInstance<QuotaObserverManager>();
        public static ToastManager ToastManager => ServiceLocator.Current.GetInstance<ToastManager>();

        public static IEncryptionService EncryptionService => ServiceLocator.Current.GetInstance<IEncryptionService>();
        public static IDataService DataService => ServiceLocator.Current.GetInstance<IDataService>();

        public static YesContext YesContext => ServiceLocator.Current.GetInstance<YesContext>();
        public static YesSessionUpdater YesSessionUpdater => ServiceLocator.Current.GetInstance<YesSessionUpdater>();
    }
}
