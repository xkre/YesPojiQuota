using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.ViewModels
{
    public class SecondPageViewModel : MainViewModel
    {
        public SecondPageViewModel(INavigationService ns) : base(ns)
        {
            this.PageTitle = "Second Page";
        }

        private RelayCommand _navigateBack;

        public RelayCommand NavigateBack
        {
            get { return _navigateBack ?? (_navigateBack = new RelayCommand(() => _navigationService.GoBack())); }
        }
    }
}
