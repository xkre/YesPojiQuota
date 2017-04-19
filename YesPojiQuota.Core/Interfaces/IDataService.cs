using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Data;

namespace YesPojiQuota.Core.Interfaces
{
    public interface IDataService
    {
        IEnumerable<Account> Accounts { get; }
    }
}
