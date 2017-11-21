using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Models;
using YesPojiUtmLib.Models;
using YesPojiUtmLib.Services;

namespace YesPojiQuota.Core.Services
{
    public class QuotaServiceMock : IYesQuotaService
    {
        private double maxQuota = 20480.00;

        public double GetMaxQuota(string username)
        {
            return maxQuota;
        }
        
        public Task<double> GetQuotaAsync(string username)
        {
            Random b = new Random();

            return Task.Run(() => b.NextDouble() * maxQuota);
        }

        public Task<double> GetQuotaAsync(YesAccount a)
        {
            Random b = new Random();

            return Task.Run(() => b.NextDouble() * maxQuota);
        }
    }
}
