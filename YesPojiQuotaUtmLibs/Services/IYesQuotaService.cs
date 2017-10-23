using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Models;

namespace YesPojiQuotaUtmLibs.Services
{
    interface IYesQuotaService
    {
        Task<double> GetQuotaAsync(string username);
        Task<double> GetQuotaAsync(Account a);

        double GetMaxQuota(string Username);
    }
}
