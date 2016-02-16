using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrcMessageBot
{
    /// <summary>
    /// A message to be sent via the !tell command
    /// </summary>
    class TellMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime SentOn { get; set; }
    }
}
