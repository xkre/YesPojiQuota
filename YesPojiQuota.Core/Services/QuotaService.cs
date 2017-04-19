using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YesPojiQuota.Core.Utils.Interfaces;

namespace YesPojiQuota.Core.Utils.Services
{
    public class QuotaService : IQuotaService
    {
        private const string URL = "http://quota.utm.my/balance.php";

        public async Task<decimal> GetQuota(string username)
        {
            decimal quota = 0;
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Username", username),
                });

                var result = await client.PostAsync(URL, content);
                var rawHtml = await result.Content.ReadAsStringAsync();

                quota = ProcessQuota(rawHtml);
            }

            return quota;
        }

        public double GetMaxQuota(string Username)
        {
            return 20 * 1024;
        }

        private decimal ProcessQuota(string rawHtml)
        {
            var result = Regex.Match(rawHtml, @"Data:([^)]*) Mega").Groups[1].Value;

            try
            {
                var quota = decimal.Parse(result);
                return quota;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception {e}");
            }

            throw new Exception("Cannot Process Quota");
        }
    }
}
