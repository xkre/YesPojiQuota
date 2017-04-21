using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<Account> Accounts => throw new NotImplementedException();
    }
}
