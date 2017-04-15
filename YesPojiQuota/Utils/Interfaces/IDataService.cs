﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Data;

namespace YesPojiQuota.Utils.Interfaces
{
    public interface IDataService
    {
        IEnumerable<Account> Accounts { get; }
    }
}