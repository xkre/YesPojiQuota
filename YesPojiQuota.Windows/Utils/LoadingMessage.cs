using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Utils
{
    internal class LoadingMessage : MessageBase
    {
        public bool IsLoading { get; set; }
        public string Message { get; set; }
    }
}
