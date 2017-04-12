using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using Windows.ApplicationModel;

namespace YesPojiQuota.ViewModels
{
    public class SettingsViewModel : MainViewModel
    {
        public SettingsViewModel(INavigationService ns) : base(ns)
        {
        }

        public string Version => "Version: " + GetVersionNumber();

        private string GetVersionNumber()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }


    }
}
