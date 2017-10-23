using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.CoreLibs.Data;
using YesPojiQuota.CoreLibs.Interfaces;
using YesPojiQuota.CoreLibs.Models;

namespace YesPojiQuota.CoreLibs.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<Account> Accounts => throw new NotImplementedException();
    }
}
