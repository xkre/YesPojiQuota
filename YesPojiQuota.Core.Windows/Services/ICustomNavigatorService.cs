﻿using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Windows.Services
{
    public interface ICustomNavigationService : INavigationService
    {
        bool CanGoBack();

        bool GoBackSystem();
    }
}
