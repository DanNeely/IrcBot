using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrcMessageBot
{
    class UserSeenStatus
    {
        public string ActivityType { get; set; }
        public string ActivityText { get; set; }
        public DateTime TimeSeen { get; set; }
    }
}
