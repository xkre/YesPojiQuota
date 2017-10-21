using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Interfaces
{
    public interface ILoginService
    {
        event LoginFailedEvent OnLoginFailed;
        event SimpleEvent OnLoginSuccess;

        //Task<bool> LoginAsync(Account a);
        Task LoginAsync(Account A);
        Task LoginAsync(string u, string p);
        Task<bool> LogoutAsync();

        Task InitAsync();       
    }
}
