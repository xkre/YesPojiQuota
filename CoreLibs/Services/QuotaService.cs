using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YesPojiQuota.CoreLibs.Interfaces;
using YesPojiQuota.CoreLibs.Models;

namespace YesPojiQuota.CoreLibs.Services
{
    public class QuotaService : IQuotaService
    {
        private const string URL = "http://quota.utm.my/balance.php";

        public async Task<double> GetQuota(string username)
        {
            double quota = 0;
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

        private double ProcessQuota(string rawHtml)
        {
            var result = Regex.Match(rawHtml, @"Data:([^)]*) Mega").Groups[1].Value;

            try
            {
                return double.Parse(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception in ProcessQuota {e}");
            }

            throw new Exception("Cannot Process Quota");
        }

        public Task<double> GetQuota(Account a)
        {
            throw new NotImplementedException();
        }
    }
}
