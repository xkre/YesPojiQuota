using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Utils.Interfaces
{
    public interface ILoginService
    {
        Task<bool> Login(string u, string p);
        string GetKey();

        Task<bool> IsOnline();
        bool CanLogin();
    }
}
