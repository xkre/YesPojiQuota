using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Models
{
    public class SessionData
    {
        public double Sent { get; set; }
        public double Received { get; set; }
        public TimeSpan Time { get; set; }
    }
}
