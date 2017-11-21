using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiUtmLib.Enums;

namespace YesPojiQuota.Utils
{
    internal class LoginMessage : MessageBase
    {
        public LoginStatus Status { get; private set; }
        public LoginMessage(LoginStatus status)
        {
            Status = status;
        }
    }
}
