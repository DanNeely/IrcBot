using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IrcDotNet;
using IrcDotNet.Samples.Common;
using Newtonsoft.Json;

namespace IrcMessageBot
{
    public class IrcMessageBot : BasicIrcBot
    {
        private const string quitMessage = "";

        private List<TellMessage> Messages = new List<TellMessage>();

        // Bot statistics
        private DateTime launchTime = DateTime.Now;
        private bool onInitialConnect = false;

        public IrcMessageBot()
            : base()
        {
            LoadMessages();
            if (ConfigSettings.ServerName.Length > 0)
            {
                onInitialConnect = true;
                Connect(ConfigSettings.ServerName, RegistrationInfo);
            }

        }

        public override IrcRegistrationInfo RegistrationInfo
        {
            get
            {
                return new IrcUserRegistrationInfo()
                {
                    NickName = ConfigSettings.NickName,
                    UserName = ConfigSettings.UserName,
                    RealName = ConfigSettings.RealName
                };
            }
        }

        public override string QuitMessage
        {
            get { return quitMessage; }
        }

        protected override void OnClientConnect(IrcClient client) 
        {
            if (onInitialConnect)
            {
                onInitialConnect = false;
                client.Channels.Join(ConfigSettings.ChannelName);
            }
        }

        protected override void OnClientDisconnect(IrcClient client)
        {
            //
        }

        protected override void OnClientRegistered(IrcClient client)
        {
            //
        }

        protected override void OnLocalUserJoinedChannel(IrcLocalUser localUser, IrcChannelEventArgs e)
        {
            DeliverMessages(e.Channel, localUser.NickName);
        }

        protected override void OnLocalUserLeftChannel(IrcLocalUser localUser, IrcChannelEventArgs e)
        {
            //
        }

        protected override void OnLocalUserNoticeReceived(IrcLocalUser localUser, IrcMessageEventArgs e)
        {
            //
        }

        protected override void OnLocalUserMessageReceived(IrcLocalUser localUser, IrcMessageEventArgs e)
        {
            //
        }

        /// <summary>
        /// Responds to a user joining the channel by checking to see if they have any messages to deliver.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="e"></param>
        protected override void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            DeliverMessages(channel, e.ChannelUser.User.NickName);
        }

        protected override void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            //
        }

        protected override void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            //
        }

        /// <summary>
        /// Responds to a user talking in the channel by checking to see if they have any messages to deliver.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="e"></param>
        protected override void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            if (e.Source is IrcUser)
            {
                DeliverMessages(channel, e.Source.Name);
            }
        }

        /// <summary>
        /// Delivers messages to the recipient if they exist.
        /// </summary>
        /// <param name="channel">The channel to deliver the message to.</param>
        /// <param name="recipent">The potential recipent of messages.</param>
        private void DeliverMessages(IrcChannel channel, string recipent)
        {
            IrcClient client = channel.Client;
            bool messageDelivered = false;

            //using Messages.ToArray() to have an stable copy for iterating over while modifying the original because you can't easily iterate a collection while modifying it.
            foreach (TellMessage message in Messages.ToArray())
            {
                if (recipent.Equals(message.To, StringComparison.OrdinalIgnoreCase))
                {
                    client.LocalUser.SendMessage(channel, $"{recipent}, {message.From} said:  {message.Message} ({message.SentOn.ToString("yyyy/MM/dd HH:mm:ss tt")})");
                    Messages.Remove(message);
                    messageDelivered = true;
                }
            }

            if (messageDelivered)
                SaveMessages();
        }


        protected override void InitializeChatCommandProcessors()
        {
            base.InitializeChatCommandProcessors();

            this.ChatCommandProcessors.Add("seen", ProcessChatCommandSeen);
            this.ChatCommandProcessors.Add("tell", ProcessChatCommandTell);
            this.ChatCommandProcessors.Add("stats", ProcessChatCommandStats);
        }

        #region Chat Command Processors

        private void ProcessChatCommandSeen(IrcClient client, IIrcMessageSource source,
            IList<IIrcMessageTarget> targets, string command, IList<string> parameters)
        {
            client.LocalUser.SendMessage(targets, "I see nothing, nothing!");
        }

        /// <summary>
        /// Responds to a !tell command by storing the message, or reporting an error if the input isn't valid.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="source"></param>
        /// <param name="targets"></param>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        private void ProcessChatCommandTell(IrcClient client, IIrcMessageSource source,
            IList<IIrcMessageTarget> targets, string command, IList<string> parameters)
        {
            if (parameters.Count >= 2)
            {
                TellMessage message = new TellMessage
                {
                    From = source.Name,
                    To = parameters[0],
                    Message = string.Join(" ", parameters.Skip(1)),
                    SentOn = DateTime.Now
                };

                Messages.Add(message);
                client.LocalUser.SendMessage(targets, $"Okay, {source.Name}");
                SaveMessages();
                return;
            }
            client.LocalUser.SendMessage(targets, "Error:  Syntax must be:  \"!tell Nickname message\"");
        }

        private void ProcessChatCommandStats(IrcClient client, IIrcMessageSource source,
            IList<IIrcMessageTarget> targets, string command, IList<string> parameters)
        {
            // Send reply with bot statistics.
            var replyTargets = GetDefaultReplyTarget(client, source, targets);

            client.LocalUser.SendNotice(replyTargets, "Bot launch time: {0:f} ({1:g} ago)",
                this.launchTime,
                DateTime.Now - this.launchTime);
        }

        #endregion

        protected override void InitializeCommandProcessors()
        {
            base.InitializeCommandProcessors();
        }

        #region Command Processors

        //

        #endregion

        /// <summary>
        /// Saves !tell messages to a json file.
        /// </summary>
        private void SaveMessages()
        {
            File.WriteAllText(ConfigSettings.MessageFileName, JsonConvert.SerializeObject(Messages));
        }

        /// <summary>
        /// Loads !tell messages from a json file.
        /// </summary>
        private void LoadMessages()
        {
            if (File.Exists(ConfigSettings.MessageFileName))
             Messages = JsonConvert.DeserializeObject<List<TellMessage>>(File.ReadAllText(ConfigSettings.MessageFileName));
        }
    }
}
