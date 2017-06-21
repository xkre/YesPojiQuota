using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Diagnostics;
using Windows.UI.Popups;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Data;
using Microsoft.Practices.ServiceLocation;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Enums;

namespace YesPojiQuota.Core.Services
{
    public class LoginService : ILoginService
    {
        private const string PORTAL_TEST_URL = "http://detectportal.firefox.com/success.txt";
        private const string LOGOUT_URL = "http://ap.logout";
        private const string LOGIN_URL = "https://apc.aptilo.com/cgi-bin/login";

        public event LoginFailedEvent OnLoginFailed;
        public event SimpleEvent OnLoginSuccess;

        private string key;
        private string rawHtml;

        private INetworkService _ns;

        public LoginService(INetworkService ns)
        {
            _ns = ns;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("showsession", "yes"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("key", key),
                    new KeyValuePair<string, string>("password", password),

                    //new KeyValuePair<string, string>("realm", "live.utm.my"),
                    //new KeyValuePair<string, string>("acceptedurl",""),
                    //new KeyValuePair<string, string>("user", "makram23"),
                });

                try
                {
                    var response = await client.PostAsync(LOGIN_URL, content);
                    var rawHtml = await response.Content.ReadAsStringAsync();

                    bool success = ParseSuccess(rawHtml);

                    if (success)
                    {
                        OnLoginSuccess();
                    }
                    else
                    {
                        var failReason = ParseFailReason(rawHtml);
                        OnLoginFailed(failReason);
                    }

                    return success;
                }
                catch (HttpRequestException ex)
                {
                    var failReason = (LoginFailureReason)100;
                    OnLoginFailed(failReason);

                    Debug.WriteLine($"Exception {ex}");
                }
            }

            return false;
        }

        public Task<bool> LoginAsync(Account a)
        {
            return LoginAsync(a.Username, a.Password);
        }

        public async Task<bool> LogoutAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(LOGOUT_URL);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"Exception {ex} on LogoutAsync()");
                }
            }

            return false;
        }

        public async Task InitAsync()
        {
            if (await TryGetLoginPortalAsync())
            {
                key = ExtractKey(rawHtml);
            }
        }

        private async Task<bool> TryGetLoginPortalAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    rawHtml = await client.GetStringAsync(PORTAL_TEST_URL);

                    if (rawHtml.Contains("success"))
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Exception {e}");
                }
                return false;
            }
        }

        private bool ParseSuccess(string rawHtml)
        {
            if (rawHtml.Contains("showsession"))
            {
                return true;
            }

            return false;
        }

        private string ExtractKey(string rawHhtml)
        {
            string theKey = String.Empty;

            var html = new HtmlDocument();
            html.LoadHtml(rawHhtml);

            theKey += html.DocumentNode.ChildNodes[2].ChildNodes[7].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "failed");
            theKey = Regex.Match(theKey, @"key=([^)]*)\&").Groups[1].Value;

            return theKey;
        }

        private LoginFailureReason ParseFailReason(string rawHTML)
        {
            //Searches for parseCause( and get the value after it
            //Might be better for performance to separate the html by line first.
            var reason = Regex.Match(rawHTML, @"parseCause\(([^)]*)").Groups[1].Value;

            //Remove "
            reason = reason.Replace('\"', ' ');
            reason = reason.Trim();

            LoginFailureReason parsedReason = (LoginFailureReason)int.Parse(reason);

            return parsedReason;
        }
    }
}
