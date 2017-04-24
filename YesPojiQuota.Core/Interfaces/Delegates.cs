using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Enums;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Interfaces
{
    public class Delegates
    {
    }

    public delegate void NetworkChangeEvent(NetworkCondition condition);
    public delegate void SessionDataUpdateEvent(SessionData data);

}
