using System;
using System.Collections.Generic;
using System.Text;
using YesPojiQuotaUtmLibs.Enums;
using YesPojiQuotaUtmLibs.Models;

namespace YesPojiQuotaUtmLibs.Events
{
    public class Events
    {
        public delegate void SessionDataUpdateEvent(SessionData data);
        public delegate void LoginFailedEvent(LoginFailureReason reason);

        public delegate void SimpleEvent();
    }
}
