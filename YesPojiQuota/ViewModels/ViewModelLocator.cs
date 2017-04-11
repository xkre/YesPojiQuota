using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Data;
using YesPojiQuota.Utils;
using YesPojiQuota.Views;

namespace YesPojiQuota.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterViewModels();
            RegisterServices();
        }

        private void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<SecondPageViewModel>();
            SimpleIoc.Default.Register<AccountsViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            //SimpleIoc.Default.Register<AddZoneViewModel>();
            //SimpleIoc.Default.Register<ZoneViewModel>();
        }


        private void RegisterServices()
        {
            var nav = new CustomNavigationService(new NavigationService());

            nav.Configure(ViewModelKeys.SETTINGS_PAGE, typeof(SettingsPage));
            //nav.Configure(ViewModelKeys.SECOND_PAGE, typeof(SecondPage));
            //nav.Configure(ViewModelKeys.ADD_ZONE_PAGE, typeof(AddZone));
            //nav.Configure(ViewModelKeys.ZONE_PAGE, typeof(ZonePage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<ICustomNavigationService>(() => nav);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<ILoginService, LoginService>();
            SimpleIoc.Default.Register<IQuotaService, QuotaServiceMock>();

            SimpleIoc.Default.Register<YesContext, YesContext>();

            //SimpleIoc.Default.Register<IZoneManager>(() => new ZoneManager());
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

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public MainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();
        public SecondPageViewModel SecondPage => ServiceLocator.Current.GetInstance<SecondPageViewModel>();
        public AccountsViewModel Accounts => ServiceLocator.Current.GetInstance<AccountsViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        //public AddZoneViewModel AddZone
        //{
        //    get { return ServiceLocator.Current.GetInstance<AddZoneViewModel>(); }
        //}

        //public ZoneViewModel Zone
        //{
        //    get { return ServiceLocator.Current.GetInstance<ZoneViewModel>(); }
        //}

        public static void Cleanup()
        {
            //TODO
            throw new NotImplementedException();
        }
    }

    public static class ViewModelKeys
    {
        public const string SETTINGS_PAGE = "SettingsPage";
        //public const string SECOND_PAGE = "SecondPage";
        //public const string ADD_ZONE_PAGE = "AddZone";
        //public const string ZONE_PAGE = "Zone";
    }
}
