﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.CoreLibs.Enums
{
    public enum NetworkCondition
    {
        Online,
        OnlineNotYes,
        YesWifiConnected,
        NotConnected,
        Undetermined = -1
    }
}
