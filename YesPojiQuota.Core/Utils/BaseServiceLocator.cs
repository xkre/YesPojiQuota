using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Utils
{
    public abstract class BaseServiceLocator
    {
        protected BaseServiceLocator()
        {
            RegisterService();
        }

        protected virtual void RegisterService()
        {
            SimpleIoc.Default.Register<IYesLoginService, YesLoginService>();
            SimpleIoc.Default.Register<IYesQuotaService, YesQuotaService>();
            SimpleIoc.Default.Register<IYesNetworkService, YesNetworkService>();
            SimpleIoc.Default.Register<IYesSessionService, YesSessionService>();

            SimpleIoc.Default.Register<IDataService, DataService>();

            SimpleIoc.Default.Register<QuotaObserverManager>();
            SimpleIoc.Default.Register<YesSessionUpdater>();

            SimpleIoc.Default.Register<YesContext>();
        }

        public static IYesLoginService YesLoginService => SimpleIoc.Default.GetInstance<IYesLoginService>();
        public static IYesQuotaService YesQuotaService => SimpleIoc.Default.GetInstance<IYesQuotaService>();
        public static IYesNetworkService YesNetworkService => SimpleIoc.Default.GetInstance<IYesNetworkService>();
        public static IYesSessionService YesSessionService => SimpleIoc.Default.GetInstance<IYesSessionService>();

        public static IDataService DataService => SimpleIoc.Default.GetInstance<IDataService>();

        public static YesContext YesContext => SimpleIoc.Default.GetInstance<YesContext>();
        public static YesSessionUpdater YesSessionUpdater => SimpleIoc.Default.GetInstance<YesSessionUpdater>();
        public static QuotaObserverManager QuotaObserverManager => SimpleIoc.Default.GetInstance<QuotaObserverManager>();
    }
}
