using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Data;
using YesPojiQuota.Utils.Interfaces;

namespace YesPojiQuota.Utils.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<Account> Accounts => throw new NotImplementedException();
    }
}
