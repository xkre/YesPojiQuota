using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiUtmLib.Enums;

namespace YesPojiQuota.Core.Windows.Utils.Messages
{
    public class LoginMessage : MessageBase
    {
        public LoginStatus Status { get; private set; }
        public LoginMessage(LoginStatus status)
        {
            Status = status;
        }
    }
}