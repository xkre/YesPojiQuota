using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Enums;
using YesPojiQuotaUtmLibs.Models;
using static YesPojiQuotaUtmLibs.Events.Events;

namespace YesPojiQuotaUtmLibs.Services
{
    public class YesLoginService : IYesLoginService
    {
        private const string PORTAL_TEST_URL = "http://detectportal.firefox.com/success.txt";
        private const string LOGOUT_URL = "http://ap.logout";
        private const string LOGIN_URL = "https://apc.aptilo.com/cgi-bin/login";

        private string rawHtml;

        private IYesNetworkService _ns;

        public YesLoginService()
        {
            _ns = new YesNetworkService();
        }

        public async Task<LoginStatus> LoginAsync(string username, string password)
        {
            string key = await GetLoginKey();

            LoginInfo loginInfo = new LoginInfo
            {
                Username = username,
                Password = password,
                Key = key
            };

            return await DoLogin(loginInfo);
        }

        public async Task<LoginStatus> LoginAsync(Account a)
        {
            return await LoginAsync(a.Username, a.Password);
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
                catch (HttpRequestException)
                {
                }
            }

            return false;
        }

        public async Task<string> GetLoginKey()
        {
            string key = "";

            if (await TryGetLoginPortalAsync())
            {
                key = ExtractKey(rawHtml);
            }

            return key;
        }

        private async Task<LoginStatus> DoLogin(LoginInfo loginInfo)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", loginInfo.Username),
                new KeyValuePair<string, string>("password", loginInfo.Password),
                new KeyValuePair<string, string>("key", loginInfo.Key),

                new KeyValuePair<string, string>("realm", loginInfo.Realm),
                new KeyValuePair<string, string>("deniedpage", loginInfo.DeniedPage),
                new KeyValuePair<string, string>("showsession", loginInfo.ShowSession),
                new KeyValuePair<string, string>("acceptedurl",loginInfo.AcceptedUrl),
                new KeyValuePair<string, string>("user", loginInfo.User),
            });

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(LOGIN_URL, content);
                    var rawHtml = await response.Content.ReadAsStringAsync();

                    bool success = ParseSuccess(rawHtml);

                    if (success)
                    {
                        return LoginStatus.Success;
                    }
                    else
                    {
                        var failReason = await ParseFailReasonAsync(rawHtml);
                        return failReason;
                    }
                }
                catch (HttpRequestException)
                {
                    return LoginStatus.HTTPError;
                }
            }
        }

        private async Task<bool> TryGetLoginPortalAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    rawHtml = await client.GetStringAsync(PORTAL_TEST_URL);

                    if (rawHtml.Contains("https://wifi.yes.my/pas/start"))
                        return true;
                }
                catch (Exception)
                {
                    //Debug.WriteLine($"Exception {e}");
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

            //TODO: this looks terrible...
            theKey += html.DocumentNode.ChildNodes[2].ChildNodes[7].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "failed");
            theKey = Regex.Match(theKey, @"key=([^)]*)\&").Groups[1].Value;

            return theKey;
        }

        private async Task<LoginStatus> ParseFailReasonAsync(string rawHTML)
        {
            //Searches for parseCause( and get the value after it
            //Might be better for performance to separate the html by line first.
            var loginURL = Regex.Match(rawHTML, @"replace\(([^)]*)\)").Groups[1].Value;
            loginURL = loginURL.Replace('"', ' ').Trim();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(loginURL);
                var loginHtml = await response.Content.ReadAsStringAsync();

                var reason = Regex.Match(loginHtml, @"deniedCause = parseCause\(([^)]*)").Groups[1].Value;

                //Remove "
                reason = reason.Replace('\"', ' ');
                reason = reason.Trim();

                LoginStatus parsedReason = (LoginStatus)int.Parse(reason);

                return parsedReason;
            }
        }

        private class LoginInfo
        {
            private string _username;
            public string Username
            {
                get => _username;
                set
                {
                    _username = value;
                    User = value.Split('@')[0];
                    Realm = value.Split('@')[1];
                }
            }

            public string Password { get; set; }
            public string Key { get; set; }

            public string Realm { get; private set; }
            public string DeniedPage => $"/pas/parsed/utm1/index_desktop.html?key={Key}&dummy=true";
            public string ShowSession => "yes";
            public string AcceptedUrl => $"http://detectportal.firefox.com/success.txt?username={Username}";
            public string User { get; private set; }
        }
    }
}
