using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Utils;
using YesPojiQuota.Core.Windows.Services;
using YesPojiQuota.Views;

namespace YesPojiQuota.UWP.Utils
{
    public class ViewModelNavigationConfigurer : IViewModelNavigationConfigurer
    {
        public void ConfigureNavigation()
        {
            var nav = new NavigationServiceEx();

            nav.Configure(ViewModelKeys.SETTINGS_PAGE, typeof(SettingsPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
        }
    }
}
