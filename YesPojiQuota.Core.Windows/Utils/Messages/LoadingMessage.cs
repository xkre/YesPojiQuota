﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Windows.Utils.Messages
{
    public class LoadingMessage : MessageBase
    {
        public bool IsLoading { get; set; }
        public string Message { get; set; }
    }
}