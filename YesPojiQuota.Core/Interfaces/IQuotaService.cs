using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Interfaces
{
    public interface IQuotaService
    {
        Task<double> GetQuota(string username);
        Task<double> GetQuota(Account a);

        double GetMaxQuota(string username);
    }
}
