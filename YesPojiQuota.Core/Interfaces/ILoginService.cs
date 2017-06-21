using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Interfaces
{
    public interface ILoginService
    {
        event LoginFailedEvent OnLoginFailed;
        event SimpleEvent OnLoginSuccess;

        //Task<bool> LoginAsync(Account a);
        Task<bool> LoginAsync(string u, string p);
        Task<bool> LogoutAsync();

        Task InitAsync();       
    }
}
