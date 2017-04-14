using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Utils.Interfaces
{
    public interface IQuotaService
    {
        Task<decimal> GetQuota(string Username);
        double GetMaxQuota(string Username);
    }
}
