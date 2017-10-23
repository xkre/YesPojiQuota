using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.CoreLibs.Enums;
using YesPojiQuota.CoreLibs.Models;

namespace YesPojiQuota.CoreLibs.Interfaces
{
    public class Delegates
    {
    }

    public delegate void NetworkChangeEvent(NetworkCondition condition);
    public delegate void SessionDataUpdateEvent(SessionData data);
    public delegate void LoginFailedEvent(LoginFailureReason reason);

    public delegate void SimpleEvent();
    //public delegate void SimpleBoolEvent(bool b);

}
