using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.CoreLibs.Models
{
    public class AppData
    {
        public int DbVersion { get; set; }
        public DateTime QuotaLastRefresed { get; set; }
    }
}
