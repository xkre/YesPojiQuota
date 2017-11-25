using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using Windows.ApplicationModel;
using YesPojiQuota.Core.Windows.ViewModels;

namespace YesPojiQuota.ViewModels
{
    public class SettingsViewModel : MainViewModel
    {
        public SettingsViewModel(INavigationService ns) : base(ns)
        {
        }

        #region Properties
        public string Version => "Version: " + GetVersionNumber();

        private bool _quotaRefreshEnabled;
        public bool QuotaRefreshEnabled
        {
            get { return _quotaRefreshEnabled; }
            set { Set("QuotaRefreshEnabled", ref _quotaRefreshEnabled, value); }
        }

        private bool _sessionEnabled;
        public bool SessionEnabled
        {
            get { return _sessionEnabled; }
            set { Set("SessionEnabled", ref _sessionEnabled, value); }
        }

        private int _quotaRefreshInterval;
        public int QuotaRefreshInterval
        {
            get { return _quotaRefreshInterval; }
            set { Set("QuotaRefreshInterval", ref _quotaRefreshInterval, value); }
        }

        private int _sessionRefreshInterval;
        public int SessionRefreshInterval
        {
            get { return _sessionRefreshInterval; }
            set { Set("QuotaRefreshInterval", ref _sessionRefreshInterval, value); }
        }

        #endregion Properties

        private string GetVersionNumber()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}
