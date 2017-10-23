using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Models;

namespace YesPojiQuotaUtmLibs.Services
{
    interface IYesSessionService
    {
        Task<SessionData> GetSessionDataAsync();
        Task<string> GetRawSessionDataAsync();

        SessionData ParseSession(string rawHtml);
    }
}
