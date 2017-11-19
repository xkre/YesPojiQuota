
using YesPojiUtmLib.Models;

namespace YesPojiQuota.Core.Models
{
    public class Account : IAccount
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Quota Quota { get; set; }

        public Account()
        {

        }

        public Account(string u, string p = "")
        {
            Username = u;
            Password = p;
        }
    }
}
