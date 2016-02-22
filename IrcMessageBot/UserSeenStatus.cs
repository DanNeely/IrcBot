using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrcMessageBot
{

    public static class ActivityTypeExtensions
    {
        /// <summary>
        /// Converts the enum to a friendly display value.  
        /// </summary>
        /// <remarks>Needed because you can't overide ToString() on an enum.</remarks>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToFriendlyString(this UserSeenStatus.ActivityType me)
        {
            switch (me)
            {
                case UserSeenStatus.ActivityType.Chat:
                    return "saying";
                case UserSeenStatus.ActivityType.Action:
                    return "doing";
                case UserSeenStatus.ActivityType.Join:
                    return "joining";
                case UserSeenStatus.ActivityType.Part:
                    return "parting";
                case UserSeenStatus.ActivityType.NicknameChange:
                    return "changing nickname";
                default:
                    throw new ArgumentException("Unknown enum.");
            }
        }
    }

    public class UserSeenStatus
    {
        public enum ActivityType
        {
            Chat,
            Action,
            Join,
            Part,
            NicknameChange  //Aspirational at present; eggdrop does this but not sure how to get it with this library
        }

        public UserSeenStatus()
        {
            SeenEvents = new Dictionary<ActivityType, Action>();
        }

        public string HostMask { get; set; }
        public Dictionary<ActivityType, Action> SeenEvents { get; set; }


        public string GetActivityTimeString(ActivityType activty)
        {
            return SeenEvents.ContainsKey(activty) ? (DateTime.Now - SeenEvents[activty].TimeSeen).ToString("d'd 'h'h 'm'm 's's'") + " ago" : "unknown";
        }

    }

    public class Action
    {
        public string ActivityText { get; set; }
        public DateTime TimeSeen { get; set; }
        public string Channel { get; set; }
    }
}
