using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Core.Interfaces;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using YesPojiQuota.Core.Helpers.Exceptions;

namespace YesPojiQuota.Core.Services
{
    public class YesSessionService
    {
        private string SESSION_URL = "https://apc.aptilo.com/apc/session.phtml";

        //private SessionData _lastSession;




        public async Task<SessionData> GetSessionData()
        {
            SessionData session;

            Debug.WriteLine("Updating Session Data");
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(SESSION_URL);
                var rawHtml = await result.Content.ReadAsStringAsync();

                session = ParseSession(rawHtml);
                //_lastSession = session;
            }

            return session;
        }



        public async Task<bool> IsConnectedToYesAsync()
        {
            Debug.WriteLine("In IsConnectedToYesAsync :: ");
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(SESSION_URL);
                    var rawHtml = await result.Content.ReadAsStringAsync();

                    var session = ParseSession(rawHtml);
                    return session.Received > 0 || session.Sent > 0;
                }
                catch
                {
                    throw new YesNotConnectedException("Not Connected to yes network");
                }
                
            }
        }


        private SessionData ParseSession(string rawHtml)
        {
            var htmlByLine = rawHtml.Split('\n');

            string sentS = htmlByLine[17];
            string recvS = htmlByLine[18];
            string timeS = htmlByLine[19];

            var sentString = Regex.Match(sentS, @":([^)]*) kB").Groups[1].Value;
            var recvString = Regex.Match(recvS, @":([^)]*) kB").Groups[1].Value;
            var timeString = Regex.Match(timeS, @":([^)]*)</tt").Groups[1].Value;

            try
            {
                var session = new SessionData()
                {
                    Sent = double.Parse(sentString),
                    Received = double.Parse(recvString),
                    Time = TimeSpan.Parse(timeString)
                };
                return session;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.StackTrace}");
                return null;
            }
        }

    }
}
