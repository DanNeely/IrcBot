using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrcMessageBot
{
    static class ConfigSettings
    {
        public static string ServerName => System.Configuration.ConfigurationManager.AppSettings["ServerName"];
        public static string ChannelName => System.Configuration.ConfigurationManager.AppSettings["ChannelName"];
        public static string NickName => System.Configuration.ConfigurationManager.AppSettings["NickName"];
        public static string UserName => System.Configuration.ConfigurationManager.AppSettings["UserName"];
        public static string RealName => System.Configuration.ConfigurationManager.AppSettings["RealName"];
    }
}
