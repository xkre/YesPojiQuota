using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Utils.Interfaces;

namespace YesPojiQuota.Utils.Services
{
    public class QuotaServiceMock : IQuotaService
    {
        private double maxQuota = 20480.00;
        public Task<decimal> GetQuota(string a)
        {
            Random b = new Random();

            return Task.Run(() => Convert.ToDecimal(b.NextDouble()*maxQuota));
        }

        public double GetMaxQuota(string Username)
        {
            return maxQuota;
        }
    }
}
