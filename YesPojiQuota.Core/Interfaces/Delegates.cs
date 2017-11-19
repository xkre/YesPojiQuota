using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;
using YesPojiUtmLib.Enums;
using YesPojiUtmLib.Models;

namespace YesPojiQuota.Core.Interfaces
{
    public class Delegates
    {
    }

    public delegate void NetworkChangeEvent(NetworkCondition condition);
    public delegate void SessionDataUpdateEvent(YesSessionData data);
    public delegate void LoginFailedEvent(LoginStatus reason);

    public delegate void SimpleEvent();
    //public delegate void SimpleBoolEvent(bool b);

}
