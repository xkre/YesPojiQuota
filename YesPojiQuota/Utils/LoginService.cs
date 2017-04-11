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

namespace YesPojiQuota.Utils
{
    public class LoginService : ILoginService
    {
        private string key;

        public async Task<bool> Login(string username, string password)
        {
            using (var client = new HttpClient())
            {
                HttpClient a = new HttpClient();

                var content = new FormUrlEncodedContent(new[]
                {
                    //new KeyValuePair<string, string>("realm", "live.utm.my"),

                    //new KeyValuePair<string, string>("showsession", "yes"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("key", key),
                    //new KeyValuePair<string, string>("acceptedurl",""),

                    //new KeyValuePair<string, string>("user", "makram23"),
                    new KeyValuePair<string, string>("password", password),
                });

                try
                {
                    var c = await a.PostAsync("https://apc.aptilo.com/cgi-bin/login", content);
                    var d = await c.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"Exception {ex}");

                    Messenger.Default.Send(
                        new NotificationMessageAction<string>(
                            "Connection Error",
                            reply =>
                            {
                                //PageTitle = reply;
                            }
                        )
                    );
                }

                //var dialog = new MessageDialog(c.IsSuccessStatusCode.ToString());

                a.Dispose();
            }

            return false;
        }

        public string GetKey()
        {
            return key;
        }

        public async Task<bool> IsOnline()
        {
            string rawHtml = String.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    rawHtml = await client.GetStringAsync("http://detectportal.firefox.com/success.txt");

                    if (rawHtml.Contains("success"))
                    {
                        return true;
                    }

                    ProcessKey(rawHtml);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Exception {e}");
                    return false;
                }
                return false;
            }
        }

        public bool CanLogin()
        {
            if (key.Contains("utm"))
                return true;

            return false;
        }

        private void ProcessKey(string rawHhtml)
        {
            string theKey = String.Empty;
            try
            {
                var html = new HtmlDocument();
                html.LoadHtml(rawHhtml);

                theKey += html.DocumentNode.ChildNodes[2].ChildNodes[7].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "failed");
                theKey = Regex.Match(theKey, @"key=([^)]*)\&").Groups[1].Value;

                key = theKey;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception {e}");
            }
        }
    }
}
