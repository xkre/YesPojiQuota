using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Interfaces
{
    public interface ILoginService
    {
        //Task<bool> LoginAsync(Account a);
        Task<bool> LoginAsync(string u, string p);

        Task InitAsync();       
    }
}
