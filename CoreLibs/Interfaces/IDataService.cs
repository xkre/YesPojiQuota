using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.CoreLibs.Data;
using YesPojiQuota.CoreLibs.Models;

namespace YesPojiQuota.CoreLibs.Interfaces
{
    public interface IDataService
    {
        IEnumerable<Account> Accounts { get; }
    }
}
