using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Services
{
    public class QuotaServiceMock : IQuotaService
    {
        private double maxQuota = 20480.00;
        public Task<double> GetQuota(string a)
        {
            Random b = new Random();

            return Task.Run(() => b.NextDouble()*maxQuota);
        }

        public double GetMaxQuota(string Username)
        {
            return maxQuota;
        }

        public Task<double> GetQuota(Account a)
        {
            Random b = new Random();

            return Task.Run(() => b.NextDouble() * maxQuota);
        }
    }
}
