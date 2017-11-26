using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
using YesPojiQuota.Core.Utils;
using YesPojiQuota.Core.ViewModels;
using YesPojiQuota.Core.Windows.Services;
using YesPojiQuota.Core.Windows.Utils;
using YesPojiQuota.Core.Windows.ViewModels;
using YesPojiQuota.Utils;
using YesPojiQuota.Views;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.ViewModels
{
    public class ViewModelLocator
    {
        private AppServiceLocator _serviceLocator;
        public ViewModelLocator()
        {
            RegisterServices();
            RegisterViewModels();

            _serviceLocator = new AppServiceLocator();
        }

        private void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<AccountsPanelViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<StatusViewModel>();

            //Multiple Instance ViewModel
            SimpleIoc.Default.Register<QuotaViewModel>();
            SimpleIoc.Default.Register<AccountViewModel>();
        }

        private void RegisterServices()
        {
            var nav = new NavigationServiceEx();

            nav.Configure(ViewModelKeys.SETTINGS_PAGE, typeof(SettingsPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<IDispatcherHelper, DispatcherHelperEx>();
        }

        /*
        private async void Init()
        {
            await ServiceLocator.Current.GetInstance<ILoginService>().InitAsync();
            await ServiceLocator.Current.GetInstance<MainPageViewModel>().InitAsync();
        }

        public MainViewModel GetViewModel(Type viewModel, string key)
        {
            var vm = ServiceLocator.Current.GetInstance(viewModel, key);
            if (vm is MainViewModel)
                return vm as MainViewModel;

            return null;
        }

        public T GetViewModel<T>(string key)
        {
            var vm = ServiceLocator.Current.GetInstance<T>(key);
            return vm;
        }
        */
        public static INavigationService NavigationService => SimpleIoc.Default.GetInstance<INavigationService>();

        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
        public MainPageViewModel MainPage => SimpleIoc.Default.GetInstance<MainPageViewModel>();
        public AccountsPanelViewModel Accounts => SimpleIoc.Default.GetInstance<AccountsPanelViewModel>();
        public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();
        public StatusViewModel InnAppToast => SimpleIoc.Default.GetInstance<StatusViewModel>();
    }
}
