using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Observers;
using YesPojiQuota.Core.Services;
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
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
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
            SimpleIoc.Default.Register<InnAppToastViewModel>();

            //Multiple Instance ViewModel
            SimpleIoc.Default.Register<QuotaViewModel>();
            SimpleIoc.Default.Register<AccountViewModel>();
        }

        private void RegisterServices()
        {
            var nav = new CustomNavigationService(new NavigationService());

            nav.Configure(ViewModelKeys.SETTINGS_PAGE, typeof(SettingsPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<ICustomNavigationService>(() => nav);
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

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public MainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();
        public AccountsPanelViewModel Accounts => ServiceLocator.Current.GetInstance<AccountsPanelViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public InnAppToastViewModel InnAppToast => ServiceLocator.Current.GetInstance<InnAppToastViewModel>();

        public static void Cleanup()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
