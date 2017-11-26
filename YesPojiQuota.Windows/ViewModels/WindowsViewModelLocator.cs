using GalaSoft.MvvmLight.Ioc;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Utils;
using YesPojiQuota.Core.ViewModels;
using YesPojiQuota.Core.Windows.Utils;
using YesPojiQuota.Core.Windows.ViewModels;
using YesPojiQuota.UWP.Utils;

namespace YesPojiQuota.UWP.ViewModels
{
    public class WindowsViewModelLocator : ViewModelLocator
    {
        public WindowsViewModelLocator() : base()
        {
        }

        protected override void RegisterViewModels()
        {
            base.RegisterViewModels();

            SimpleIoc.Default.Register<SettingsViewModel>();   
        }

        protected override void RegisterServices()
        {
            var serviceLocator = new AppServiceLocator();

            SimpleIoc.Default.Register<BaseServiceLocator>(() => serviceLocator);
            SimpleIoc.Default.Register(() => serviceLocator);

            base.RegisterServices();

            SimpleIoc.Default.Register<IViewModelNavigationConfigurer, ViewModelNavigationConfigurer>();
        }

        public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();
    }
}
