using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuotaUtmLibs.Enums;
using YesPojiQuotaUtmLibs.Models;

namespace YesPojiQuotaUtmLibs.Services
{
    interface IYesLoginService
    {
        Task<LoginStatus> LoginAsync(string username, string password);
        Task<LoginStatus> LoginAsync(Account a);

        Task<bool> LogoutAsync();

        Task<String> GetLoginKey();
    }
}
