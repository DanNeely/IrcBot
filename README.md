
Intended as a utility bot for one of my channels.

This's build around the IrcDotNet framework and is derived from one of the samples included with the framework code.

https://github.com/IrcDotNet/IrcDotNet

Has working !tell and !seen commands.  The former is complete, the latter still falls short of what an eggdrop supports.

The current implementation's not production ready.  Primarily due to issues 2 and 3 on my todo list.

TODO

1) Detect Nickname changes for !seen command.

2) Add support for additional advanced !seen commands - will probably require storing more data in the json.

3) Figure out why I'm getting a crash in the framework when a server restarts.  I can't fake this one by breaking 
a network connection.  I've got a copy of one of their samples running in a debugger now.  The next server event should
at least tell me if it's a bug in the IrcDotNet framework; or if I didn't set something up in my coode.

4) Remove residual console commands inherited from the sample bot.
