using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Models;

namespace YesPojiQuotaUtmLibs.Services
{
    public class YesQuotaService : IYesQuotaService
    {
        private const string QUOTA_URL = "http://quota.utm.my/balance.php";

        public async Task<double> GetQuotaAsync(string username)
        {
            double quota = 0;
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Username", username),
                });

                var result = await client.PostAsync(QUOTA_URL, content);
                var rawHtml = await result.Content.ReadAsStringAsync();

                quota = ProcessQuota(rawHtml);
            }

            return quota;
        }

        public async Task<double> GetQuotaAsync(Account a)
        {
            return await GetQuotaAsync(a.Username);
        }

        public double GetMaxQuota(string Username)
        {
            return 20 * 1024;
        }

        private double ProcessQuota(string rawHtml)
        {
            var result = Regex.Match(rawHtml, @"Data:([^)]*) Mega").Groups[1].Value;

            try
            {
                return double.Parse(result);
            }
            catch (Exception)
            {
                //Debug.WriteLine($"Exception in ProcessQuota {e}");
            }

            throw new Exception("Cannot Process Quota");
        }
    }
}
