/*
 * Thresher IRC client library
 * Copyright (C) 2002 Aaron Hunter <thresher@sharkbite.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 * 
 * See the gpl.txt file located in the top-level-directory of
 * the archive of this library for complete text of license.
*/

#if DEBUG

using System;
using NUnit.Framework;


namespace Sharkbite.Irc.Test
{
	/// <summary>
	/// Test Listener
	/// </summary>
	[TestFixture]
	public class ListenerTest 
	{

		private string[] testLines;
		private Listener listener;

		[SetUp]
		public void SetUp() 
		{ 
			listener = new Listener();
			RegisterListeners();
			AssignLines();
		}

		private void RegisterListeners() 
		{
			listener.OnAway += new AwayEventHandler( OnAway );
			listener.OnError += new ErrorMessageEventHandler( OnError );
			listener.OnInvite +=new InviteEventHandler( OnInvite );
			listener.OnInviteSent +=new InviteSentEventHandler( OnInviteSent );
			listener.OnIson +=new IsonEventHandler( OnIson );
			listener.OnJoin +=new JoinEventHandler( OnJoin );
			listener.OnKick +=new KickEventHandler( OnKick );
			listener.OnList +=new ListEventHandler( OnList );
			listener.OnNames+=new NamesEventHandler( OnNames );
			listener.OnNick +=new NickEventHandler( OnNick );
			listener.OnNickError +=new NickErrorEventHandler( OnNickError );
			listener.OnPublicNotice +=new PublicNoticeEventHandler( OnPublicNotice );
			listener.OnPrivateNotice +=new PrivateNoticeEventHandler( OnPrivateNotice );
			listener.OnPart +=new PartEventHandler( OnPart );
			listener.OnPing +=new PingEventHandler( OnPing );
			listener.OnPrivate +=new PrivateMessageEventHandler( OnPrivate );
			listener.OnPublic +=new PublicMessageEventHandler( OnPublic );
			listener.OnQuit +=new QuitEventHandler( OnQuit );
			listener.OnRegistered +=new RegisteredEventHandler( OnRegistered );
			listener.OnReply +=new ReplyEventHandler( OnReply );
			listener.OnTopicChanged +=new TopicEventHandler( OnTopicChanged );
			listener.OnTopicRequest+=new TopicRequestEventHandler( OnTopicRequest );
			listener.OnWho +=new WhoEventHandler( OnWho );
			listener.OnWhois += new WhoisEventHandler( OnWhois );
			listener.OnWhowas += new WhowasEventHandler( OnWhowas );
			listener.OnAction += new ActionEventHandler( OnAction );
			listener.OnUserModeChange += new UserModeChangeEventHandler( OnUserModeChange );
			listener.OnUserModeRequest += new UserModeRequestEventHandler( OnUserModeRequest );
			listener.OnChannelModeRequest += new ChannelModeRequestEventHandler( OnChannelModeRequest );
			listener.OnChannelModeChange += new ChannelModeChangeEventHandler( OnChannelModeChange );
			listener.OnChannelList += new ChannelListEventHandler( OnChannelList );
			listener.OnPrivateAction += new PrivateActionEventHandler( OnPrivateAction );
			listener.OnVersion += new VersionEventHandler( OnVersion );
			listener.OnMotd += new MotdEventHandler( OnMotd );
			listener.OnTime += new TimeEventHandler( OnTime );
			listener.OnInfo += new InfoEventHandler( OnInfo );
			listener.OnAdmin += new AdminEventHandler( OnAdmin );
			listener.OnLusers += new LusersEventHandler( OnLusers );
			listener.OnLinks += new LinksEventHandler( OnLinks );
			listener.OnStats += new StatsEventHandler( OnStats );
			listener.OnKill += new KillEventHandler( OnKill );
		}

		
		private void AssignLines() 
		{
			testLines = new string[]{
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG #sharktest :\x0001ACTION Looks around\x0001", //OnAction
				":irc.sventech.com 301 alphaX234 Scurvy :is away: (Be right back) [BX-MsgLog On]",//OnAway
				":irc.sventech.com 401 alphaX234 foxtrot :No such nick/channel",//OnError
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net INVITE alphaX234 :#sharktest",//OnInvite
				":irc.sventech.com 341 alphaX234 Scurvy #sharktest",//OnInviteSent
				":irc.sventech.com 303 alphaX234 :Scurvy",	//OnIson
				":alphaX234!~alphaX234@pcp825822pcs.nrockv01.md.comcast.net JOIN :#sharktest",//OnJoin
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net KICK #sharktest alphaX234 :Go away",//OnKick
				":irc.sventech.com 322 alphaX234 #sun 17 :Send criticisms of new sun.com website to magellan-questions@sun.com - like http://wwws.sun.com/software/star/gnome/jtf/ is grossly wrong ?",//OnList
				":irc.sventech.com 353 alphaX234 = #sharktest :alphaX234 @Scurvy",//OnNames
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net NICK :MudBall",//OnNick
				":irc.sventech.com 433 alphaX234 Scurvy :Nickname is already in use.",//OnNickError
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PART #sharktest :Later all",//OnPart
				"PING :irc.sventech.com",//OnPing
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG alphaX234 :Hätte ich das früher gewusst, müsste ich jetzt wohl nicht das Öl wechseln. Scheiße",//OnPrivate
				":Scurvyaaaaaaaaa!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net NOTICE alphaX234 :Private notice",//OnPrivateNotice
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG #sharktest :  Test  of public-message! ",//OnPublic
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net NOTICE #sharktest :public notice", //OnPubNotice
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net QUIT :Bye all",//OnQuit
				":irc.sventech.com 001 alphaX234 :Welcome to IRC", //OnRegistered
				":washington.dc.us.undernet.org 381 EchoBot PREFIX=(ov)@+ CHANMODES=b,k,l,imnpst CHARSET=rfc1459 NETWORK=Undernet :are supported by this server", //OnReply
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net TOPIC #sharktest :A new topic",//OnTopicChanged
				":irc.sventech.com 332 alphaX234 #sharktest :A new topic",//OnTopicRequest
				":irc.sventech.com 352 alphaX234 #sharktest ~Scurvy pcp825822pcs.nrockv01.md.comcast.net irc.sventech.com Scurvy H@ :0 Scurvy",//OnWho
				//-->Start Whois set
				":irc.sventech.com 311 alphaX234 Scurvy ~Scurvy pcp825822pcs.nrockv01.md.comcast.net * :Scurvy",
				":irc.sventech.com 319 alphaX234 Scurvy :@#sharktest",
				":irc.sventech.com 312 alphaX234 Scurvy irc.sventech.com :GIMPnet IRC Server",
				":irc.sventech.com 317 alphaX234 Scurvy 0 1018611059 :seconds idle, signon time",
				":irc.sventech.com 318 alphaX234 Scurvy :End of /WHOIS list.",
				//<--End whois
				":irc.sventech.com 314 alphaX234 Scurvy ~Scurvy pcp825822pcs.nrockv01.md.comcast.net * :Scurvy",//OnWhowas
				":deltabot MODE deltabot :+w", //OnUserModeChange
				":irc.sventech.com 221 deltabot +iw", //OnUserModeRequest
				":irc.sventech.com 324 deltabot #sharktest +tpl 10", //OnChannelModeRequest
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net MODE #sharktest +h bob +b-i *!*@* +e *!*@*.fi", //OnChannelModeChanged
				":irc.sventech.com 367 deltabot #sharktest jocko!*@* Scurvy 120192129", //OnChannelList 
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG nick :\x0001ACTION Looks around\x0001", //OnPrivateAction
				":irc.sventech.com 351 alphaX234 IRC: 1.203b ircd :some great comments", //OnVersion
				":my.server.name 372 EchoBot :- Sunray is the sharkbite.org test server.", //OnMotd
				":my.server.name 391 EchoBot :Sunray : 25 December 2002", //OnTime
				":my.server.name 371 EchoBot :A great piece of software", //OnInfo
				":Sunray 256 EchoBot Sunray :Administrative info", //OnAdmin
				":NuclearFallout.CA.US.GamesNET.net 252 EchoBot 47 :operator(s) online", //OnLusers
				":ircd.gimp.org 364 EchoBot irc.sventech.com irc.acc.umu.se :2 GIMPnet IRC Server", //OnLinks
				":ircd.gimp.org 242 EchoBot :Server Up 46 days, 8:47:20", //OnStats
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net KILL Bob :Useless floodbot", //OnKill,	
				};
		}

		[Test]
		public void TestParse() 
		{
			foreach( string line in testLines ) 
			{
				listener.Parse( line );
			}
		}

		public void OnNickError(string badNick, string reason) 
		{
			Assertion.AssertEquals( "OnNickError: bad nick", "Scurvy", badNick );
			Assertion.AssertEquals( "OnNickError: reason", "Nickname is already in use.", reason );
			Console.WriteLine("OnNickError");
		}

		public void OnPing(string message) 
		{
			Assertion.AssertEquals( "OnPing: message", "irc.sventech.com", message);
			Console.WriteLine("OnPing");
		}

		public void OnReply(ReplyCode code, string message) 
		{
			Assertion.AssertEquals( "OnReply: code", ReplyCode.RPL_YOUREOPER, code );
			Assertion.AssertEquals( "OnReply: message", "PREFIX=(ov)@+ CHANMODES=b,k,l,imnpst CHARSET=rfc1459 NETWORK=Undernet :are supported by this server", message );
			Console.WriteLine("OnReply");
		}

		public void OnError(ReplyCode code, string message) 
		{
			Assertion.AssertEquals("OnError message","foxtrot :No such nick/channel", message );
			Assertion.AssertEquals("OnError code", ReplyCode.ERR_NOSUCHNICK, code );
			Console.WriteLine("OnError");
		}

		public void OnAway(string user, string awayMessage) 
		{
			Assertion.AssertEquals( "OnAway: user", "Scurvy", user );
			Assertion.AssertEquals( "OnAway: message", "is away: (Be right back) [BX-MsgLog On]", awayMessage );
			Console.WriteLine("OnAway");
		}

		public void OnInviteSent(string nick, string channel) 
		{
			Assertion.AssertEquals( "OnInviteSent: user", "Scurvy", nick );
			Assertion.AssertEquals( "OnInviteSent: channel", "#sharktest", channel );
			Console.WriteLine("OnInviteSent");
		}

		public void OnTopicRequest(string channel, string topic) 
		{
			Assertion.AssertEquals("OnTopicRequest: channel", "#sharktest", channel);
			Assertion.AssertEquals("OnTopicRequest: topic", "A new topic", topic);
			Console.WriteLine("OnTopicRequest");
		}

		public void OnRegistered() 
		{
			Assertion.Assert("OnRegistered", true );
			Console.WriteLine("OnRegistered");
		}


		public void OnPublicNotice(UserInfo who, string channel, string notice) 
		{
			Assertion.AssertEquals("OnPublicNotice: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnPublicNotice: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnPublicNotice: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnPublicNotice: channel", "#sharktest", channel);
			Assertion.AssertEquals("OnPublicNotice: notice", "public notice", notice);
			Console.WriteLine("OnPublicNotice");
		}

		public void OnPrivateNotice(UserInfo who, string notice) 
		{
			Assertion.AssertEquals("OnPrivateNotice: userInfo.Nick","Scurvyaaaaaaaaa", who.Nick );
			Assertion.AssertEquals("OnPrivateNotice: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnPrivateNotice: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnPrivateNotice: notice", "Private notice", notice);
			Console.WriteLine("OnPrivateNotice");
		}


		public void OnJoin(UserInfo who, string channel) 
		{
			Assertion.AssertEquals("OnJoin: userInfo.Nick","alphaX234", who.Nick );
			Assertion.AssertEquals("OnJoin: userInfo.User","~alphaX234", who.User );
			Assertion.AssertEquals("OnJoin: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnJoin: channel", "#sharktest", channel );
			Console.WriteLine("OnJoin");
		}

		public void OnPublic(UserInfo from, string channel, string message) 
		{
			Assertion.AssertEquals("OnPublic: userInfo.Nick","Scurvy", from.Nick );
			Assertion.AssertEquals("OnPublic: userInfo.User","~Scurvy", from.User );
			Assertion.AssertEquals("OnPublic: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", from.Hostname );
			Assertion.AssertEquals("OnPublic: channel", "#sharktest", channel );
			Assertion.AssertEquals("OnPublic: message", "  Test  of public-message! ", message );
			Console.WriteLine("OnPublic");
		}

		public void OnNick(UserInfo user, string newNick) 
		{		
			Assertion.AssertEquals("OnNick: userInfo.Nick","Scurvy", user.Nick );
			Assertion.AssertEquals("OnNick: userInfo.User","~Scurvy", user.User );
			Assertion.AssertEquals("OnNick: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", user.Hostname );
			Assertion.AssertEquals("OnNick: newNick", "MudBall", newNick);
			Console.WriteLine("OnNick");
		}

		public void OnPrivate(UserInfo from, string message) 
		{
			Assertion.AssertEquals("OnPrivate: userInfo.Nick","Scurvy", from.Nick );
			Assertion.AssertEquals("OnPrivate: userInfo.User","~Scurvy", from.User );
			Assertion.AssertEquals("OnPrivate: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", from.Hostname );
			Assertion.AssertEquals("OnPrivate: message", "Hätte ich das früher gewusst, müsste ich jetzt wohl nicht das Öl wechseln. Scheiße", message );
			Console.WriteLine("OnPrivate");
		}

		public void OnTopicChanged(UserInfo who, string channel, string newTopic) 
		{
			Assertion.AssertEquals("OnTopicChanged: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnTopicChanged: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnTopicChanged: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnTopicChanged: channel", "#sharktest", channel);
			Assertion.AssertEquals("OnTopicChanged: new topic", "A new topic", newTopic);
			Console.WriteLine("OnTopicChanged");
		}

		public void OnPart(UserInfo who, string channel, string reason) 
		{
			Assertion.AssertEquals("OnPart: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnPart: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnPart: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnPart: channel", "#sharktest", channel);
			Assertion.AssertEquals("OnPart: reason", "Later all", reason);
			Console.WriteLine("OnPart");
		}

		public void OnQuit(UserInfo who, string reason) 
		{
			Assertion.AssertEquals("OnQuit: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnQuit: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnQuit: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnQuit: reason", "Bye all", reason);
			Console.WriteLine("OnQuit");
		}

		public void OnInvite(UserInfo who, string channel) 
		{
			Assertion.AssertEquals("OnInvite: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnInvite: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnInvite: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals("OnInvite: channel", "#sharktest", channel);
			Console.WriteLine("OnInvite");
		}

		public void OnKick(UserInfo kicker, string channel, string kickee, string reason) 
		{
			Assertion.AssertEquals("OnKick: userInfo.Nick","Scurvy", kicker.Nick );
			Assertion.AssertEquals("OnKick: userInfo.User","~Scurvy", kicker.User );
			Assertion.AssertEquals("OnKick: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", kicker.Hostname );
			Assertion.AssertEquals("OnKick: channel", "#sharktest", channel);
			Assertion.AssertEquals("OnKick: kickee", "alphaX234", kickee);
			Assertion.AssertEquals("OnKick: reason", "Go away", reason);
			Console.WriteLine("OnKick");
		}
 
		
		public void OnNames( string channel, string[] nicks, bool done )
		{
			Assertion.AssertEquals("OnNames channel","#sharktest", channel);
			Assertion.AssertEquals("OnNames nick[0]","alphaX234",nicks[0]);
			Assertion.AssertEquals("OnNames nick[1]","@Scurvy",nicks[1]);			
			Assertion.Assert("OnNames done", !done );
			Console.WriteLine("OnNames");
		}
 
		public void OnList( string channel, int count ,string topic, bool done )
		{			
			Assertion.AssertEquals("OnList channel","#sun", channel);
			Assertion.AssertEquals("OnList count",17, count);
			Assertion.AssertEquals("OnList topic","Send criticisms of new sun.com website to magellan-questions@sun.com - like http://wwws.sun.com/software/star/gnome/jtf/ is grossly wrong ?", topic);
			Assertion.Assert("OnList done", !done );
			Console.WriteLine("OnList");
		}
	
		public void OnIson( string nick)
		{
			Assertion.AssertEquals("OnIson nick","Scurvy", nick);
			Console.WriteLine("OnIson" );
		}

		public void OnWho( UserInfo user, string channel, string ircServer, string mask, 
			int hopCount, string realName, bool last ) 
		{
			Assertion.AssertEquals("OnWho: userInfo.Nick","Scurvy", user.Nick );
			Assertion.AssertEquals("OnWho: userInfo.User","~Scurvy", user.User );
			Assertion.AssertEquals("OnWho: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", user.Hostname );
			Assertion.AssertEquals("OnWho: channel", "#sharktest" , channel );
			Assertion.AssertEquals("OnWho: ircServer", "irc.sventech.com", ircServer );
			Assertion.AssertEquals("OnWho: mask", "H@", mask );
			Assertion.AssertEquals("OnWho: hopCount", 0, hopCount );
			Assertion.AssertEquals("OnWho: real name", "Scurvy", realName );
			Assertion.Assert("OnWho: last", !last);
			Console.WriteLine("OnWho");
		}

		public void OnWhois( WhoisInfo whoisInfo )
		{
			Assertion.AssertEquals("OnWhois: userInfo.Nick","Scurvy", whoisInfo.User.Nick );
			Assertion.AssertEquals("OnWhois: userInfo.User","~Scurvy", whoisInfo.User.User );
			Assertion.AssertEquals("OnWhois: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", whoisInfo.User.Hostname );
			Assertion.AssertEquals("OnWhois: channels", "@#sharktest", whoisInfo.GetChannels()[0]);
			Assertion.Assert("OnWhois: idle time", 1018611059 == whoisInfo.IdleTime );
			Assertion.Assert("OnWhois: operator", !whoisInfo.Operator );
			Assertion.AssertEquals("OnWhois: real name","Scurvy", whoisInfo.RealName );
			Assertion.AssertEquals("OnWhois: server","irc.sventech.com", whoisInfo.Server );
			Assertion.AssertEquals("OnWhois: server description","GIMPnet IRC Server", whoisInfo.ServerDescription );
			Console.WriteLine("OnWhois");
		}

		public void OnWhowas( UserInfo user, string realName, bool last )
		{
			Assertion.AssertEquals("OnWhowas: userInfo.Nick","Scurvy", user.Nick );
			Assertion.AssertEquals("OnWhowas: userInfo.User","~Scurvy", user.User );
			Assertion.AssertEquals("OnWhowas: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", user.Hostname );
			Assertion.AssertEquals("OnWhowas: realName", "Scurvy", realName );
			Assertion.Assert("OnWhowas: last", !last );
			Console.WriteLine("OnWhowas");
		}

		public void OnAction(UserInfo from, string channel, string description) 
		{
			Assertion.AssertEquals("OnAction: userInfo.Nick","Scurvy", from.Nick );
			Assertion.AssertEquals("OnAction: userInfo.User","~Scurvy", from.User );
			Assertion.AssertEquals("OnAction: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", from.Hostname );
			Assertion.AssertEquals("OnAction: channel", "#sharktest", channel );
			Assertion.AssertEquals("OnAction: description", "Looks around", description );
			Console.WriteLine("OnAction");
		}

		public void OnUserModeChange( ModeAction action, UserMode mode ) 
		{
			Assertion.AssertEquals("OnUserModeChange: action", ModeAction.Add, action );
			Assertion.AssertEquals("OnUserModeChange: mode", UserMode.Wallops, mode );
			Console.WriteLine("OnUserModeChange");
		}

		public void OnUserModeRequest( UserMode[] modes ) 
		{
			Assertion.AssertEquals("OnUserModeRequest: size", 2, modes.Length );
			Assertion.AssertEquals("OnUserModeRequest: invisible", UserMode.Invisible, modes[0] );
			Assertion.AssertEquals("OnUserModeRequest: wallops", UserMode.Wallops, modes[1] );
			Console.WriteLine("OnUserModeRequest");
		}

		public void OnChannelModeRequest( string channel, ChannelModeInfo[] modes ) 
		{
			//":irc.sventech.com 324 deltabot #sharktest +tpl 10"			

			Assertion.AssertEquals("OnChannelModeRequest: channel", "#sharktest", channel );
			Assertion.AssertEquals("OnChannelModeRequest: size", 3, modes.Length );

			Assertion.AssertEquals("OnChannelModeRequest: topic", ChannelMode.TopicSettable, modes[0].Mode );

			Assertion.AssertEquals("OnChannelModeRequest: private", ChannelMode.Private, modes[1].Mode );

			Assertion.AssertEquals("OnChannelModeRequest: limited", ChannelMode.UserLimit, modes[2].Mode );
			Assertion.AssertEquals("OnChannelModeRequest: param", "10", modes[2].Parameter );
			Console.WriteLine("OnChannelModeRequest");
		}
	
		public void OnChannelModeChange( UserInfo who, string channel, ChannelModeInfo[] modes) 
		{//+h bob +b -i *!*@* +e *!*@*.fi
			
			Assertion.AssertEquals("OnChannelModeChange: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnChannelModeChange: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnChannelModeChange: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			
			Assertion.AssertEquals("OnChannelModeChange: channel", "#sharktest", channel );
			Assertion.AssertEquals("OnChannelModeChange: action", ModeAction.Add, modes[0].Action );
			Assertion.AssertEquals("OnChannelModeChange: size", 4, modes.Length );

			Assertion.AssertEquals("OnChannelModeChange: halfop", ChannelMode.HalfChannelOperator, modes[0].Mode );
			Assertion.AssertEquals("OnChannelModeChange: halfop", "bob", modes[0].Parameter );

			Assertion.AssertEquals("OnChannelModeChange: ban", ChannelMode.Ban, modes[1].Mode );
			Assertion.AssertEquals("OnChannelModeChange: param", "*!*@*", modes[1].Parameter );

			Assertion.AssertEquals("OnChannelModeChange: invite action", ModeAction.Remove, modes[2].Action );
			Assertion.AssertEquals("OnChannelModeChange: invite mode", ChannelMode.InviteOnly, modes[2].Mode );

			Assertion.AssertEquals("OnChannelModeChange: exception action", ModeAction.Add, modes[3].Action );
			Assertion.AssertEquals("OnChannelModeChange: exception mode", ChannelMode.Exception, modes[3].Mode );
			Assertion.AssertEquals("OnChannelModeChange: exception param", "*!*@*.fi", modes[3].Parameter );
			Console.WriteLine("OnChannelModeChange");
			
		}


		public void OnChannelList( string channel, ChannelMode mode, string item, UserInfo who, long when, bool last ) 
		{
			Assertion.AssertEquals("OnChannelList: channel", "#sharktest", channel );
			Assertion.AssertEquals("OnChannelList: mode", ChannelMode.Ban, mode );
			Assertion.AssertEquals("OnChannelList: item", "jocko!*@*", item );
			Assertion.AssertEquals("OnChannelList: who", "Scurvy", who.Nick );
			Assertion.Assert("OnChannelList: when", 120192129 == when );
			Assertion.Assert( "OnChannelList: last", !last );
			Console.WriteLine("OnChannelList");
		}

		public void OnPrivateAction(UserInfo from, string description) 
		{
			Assertion.AssertEquals("OnPrivateAction: userInfo.Nick","Scurvy", from.Nick );
			Assertion.AssertEquals("OnPrivateAction: userInfo.User","~Scurvy", from.User );
			Assertion.AssertEquals("OnPrivateAction: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", from.Hostname );
			Assertion.AssertEquals("OnPrivateAction: description", "Looks around", description );
			Console.WriteLine("OnPrivateAction");
		}

		public void OnVersion( string versionInfo ) 
		{
			Assertion.AssertEquals("OnVersion: versionInfo","IRC: 1.203b ircd :some great comments", versionInfo );
			Console.WriteLine("OnVersion");
		}

		public void OnMotd( string message, bool last ) 
		{
			Assertion.AssertEquals("OnMotd: message ","- Sunray is the sharkbite.org test server.", message );
			Assertion.Assert("OnMotd: last ", !last );

			Console.WriteLine("OnMotd");
		}

		public void OnTime( string time ) 
		{
			Assertion.AssertEquals("OnTime: time","Sunray : 25 December 2002", time );
			Console.WriteLine("OnTime");
		}

		public void OnInfo( string message, bool done ) 
		{
			Assertion.AssertEquals("OnInfo: message","A great piece of software", message );
			Assertion.Assert("OnInfo: done", !done );
			Console.WriteLine("OnInfo");
		}

		public void OnAdmin( string message ) 
		{
			Assertion.AssertEquals("OnAdmin: message","Sunray :Administrative info", message );
			Console.WriteLine("OnAdmin");
		}

		public void OnLusers( string message ) 
		{
			Assertion.AssertEquals("OnLusers: message","47 :operator(s) online", message );
			Console.WriteLine("OnLusers");
		}

		public void OnLinks( string mask, string hostname, int hopCount, string serverInfo, bool done  ) 
		{
			Assertion.AssertEquals("OnLusers: mask","irc.sventech.com" , mask );
			Assertion.AssertEquals("OnLusers: hostname","irc.acc.umu.se", hostname );
			Assertion.AssertEquals("OnLusers: hopcount",2, hopCount );
			Assertion.AssertEquals("OnLusers: server info","GIMPnet IRC Server", serverInfo );
			Assertion.Assert("OnLusers: done",!done );
			Console.WriteLine("OnLinks");
		}

		public void OnStats( StatsQuery queryType, string message, bool done )
		{
			Assertion.AssertEquals("OnStats: queryType", StatsQuery.Uptime, queryType);
			Assertion.AssertEquals("OnStats: message", "Server Up 46 days, 8:47:20", message);
			Assertion.Assert("OnStats: done", !done);
			Console.WriteLine("OnStats");
		}

		public void OnKill( UserInfo op, string nick, string reason ) 
		{
			Assertion.AssertEquals("OnKill: userInfo.Nick","Scurvy", op.Nick );
			Assertion.AssertEquals("OnKill: userInfo.User","~Scurvy", op.User );
			Assertion.AssertEquals("OnKill: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", op.Hostname );
			Assertion.AssertEquals("OnKill: nick", "Bob", nick );
			Assertion.AssertEquals("OnKill: reason", "Useless floodbot", reason );
			Console.WriteLine("OnKill");
		}

	}
}
#endif