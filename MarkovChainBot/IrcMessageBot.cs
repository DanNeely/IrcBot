﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IrcDotNet;
using IrcDotNet.Samples.Common;

namespace MarkovChainTextBox
{
    public class IrcMessageBot : BasicIrcBot
    {
        private const string quitMessage = "Andrey Markov, 1856 - 1922";

        // Bot statistics
        private DateTime launchTime;

        public IrcMessageBot()
            : base()
        {
            this.launchTime = DateTime.Now;
        }

        public override IrcRegistrationInfo RegistrationInfo
        {
            get
            {
                return new IrcUserRegistrationInfo()
                    {
                    //TODO Move this to a config file
                        NickName = "YhDotNext",
                        UserName = "YhDotNext",
                        RealName = "YhDotNext"
                };
            }
        }

        public override string QuitMessage
        {
            get { return quitMessage; }
        }

        protected override void OnClientConnect(IrcClient client)
        {
            //
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
            //
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

        protected override void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            //
        }

        protected override void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            //
        }

        protected override void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            //
        }

        protected override void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            var client = channel.Client;

            if (e.Source is IrcUser)
            {
                //TODO - processing logic here     

            }
        }

        protected override void InitializeChatCommandProcessors()
        {
            base.InitializeChatCommandProcessors();

            this.ChatCommandProcessors.Add("talk", ProcessChatCommandTalk);
            this.ChatCommandProcessors.Add("stats", ProcessChatCommandStats);
        }

        #region Chat Command Processors

        private void ProcessChatCommandTalk(IrcClient client, IIrcMessageSource source,
            IList<IIrcMessageTarget> targets, string command, IList<string> parameters)
        {
            client.LocalUser.SendMessage(targets, "Hello world!");
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
    }
}