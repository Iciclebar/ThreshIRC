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
using System.IO;
using System.Text;
using NUnit.Framework;


namespace Sharkbite.Irc.Test
{
	/// <summary>
	/// Test Sender
	/// </summary>
	[TestFixture]
	public class SenderTest
	{

		/// <summary>
		/// A delegate that wraps a single param no colon method
		/// </summary>
		private delegate void SingleParam( string param );

		/// <summary>
		/// A delegate that wraps a single optional param method
		/// </summary>
		private delegate void SingleOptionalParam( params string[] param );

		/// <summary>
		/// A delegate wrapping a command which takes some text prefaced by a colon.
		/// </summary>
		private delegate void SingleColonParam( string text );
		
		/// <summary>
		/// A delegate wrapping a command which takes a single
		/// parameter and then some text prefaced by a colon.
		/// </summary>
		private delegate void DoubleColonParam( string param, string text );

		/// <summary>
		/// A delegate wrapping a command which takes two
		/// parameters separated by a space.
		/// </summary>
		private delegate void DoubleParam( string param, string text );
		
		private MemoryStream buffer;
		private Connection connection;
		
		[SetUp]
		public void SetUp() 
		{ 	
			ConnectionArgs args = new ConnectionArgs("deltabot","irc.sventech.com");
			buffer = new MemoryStream();
			connection = new Connection( args );
			connection.writer = new StreamWriter( buffer );
			connection.writer.AutoFlush = true;
		}

		private string CreateString( int length ) 
		{
			StringBuilder builder = new StringBuilder( length );
			for( int i = 0; i < length; i ++ ) 
			{
				builder.Append("X");
			}
			return builder.ToString();
		}

		private string BufferToString() 
		{
			buffer.Close();
			string command = Encoding.ASCII.GetString( buffer.ToArray() );
			buffer = new MemoryStream();
			connection.writer = new StreamWriter( buffer );
			connection.writer.AutoFlush = true;
			return command.TrimEnd();
		}

		private void DoubleParamTest( DoubleParam method, string command, string one, string two ) 
		{
			method(one, two );
			Assertion.AssertEquals(command + " " + one + " " + two, BufferToString() );
			try 
			{
				method(null, two );
				Assertion.Fail( "Null first parameter.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(null, null);
				Assertion.Fail( "Null first and second parameters.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				method(one, null );
				Assertion.Fail( "Null first parameter");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(one, "" );
				Assertion.Fail( "Empty first parameter");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("", "" );
				Assertion.Fail( "Empty first and second parameters");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("", two );
				Assertion.Fail( "Empty second parameter");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		private void DoubleColonParamTest( DoubleColonParam method, string command, string param, int length, bool discardLong ) 
		{
			method(param,"some text");
			Assertion.AssertEquals(command + " " + param + " :some text", BufferToString() );
			try 
			{
				method(null, "bad value");
				Assertion.Fail( "Null param");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(null, null);
				Assertion.Fail( "Null command and param.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				method(param, null );
				Assertion.Fail( "Null text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(param, "" );
				Assertion.Fail( "Empty text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("", "" );
				Assertion.Fail( "Empty command and text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("", "some text" );
				Assertion.Fail( "Empty command");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			if( discardLong ) 
			{
				method( param, CreateString( 600 ) );
				Assertion.AssertEquals( command + " " + param + " :" + CreateString(length), BufferToString());
			}
			else 
			{
				//test a message that is not broken up
				method( param, CreateString( 300 ) );
				string prefix = command + " " + param + " :";
				string sent = prefix + CreateString( 300);
				Assertion.AssertEquals( sent, BufferToString() );

				//test a message broken into two
				method( param, CreateString( 600 ) );
				sent = prefix + CreateString( length) + "\r\n";
				sent += prefix + CreateString( 600 - length ) ;
				Assertion.AssertEquals( sent, BufferToString() );

				//test a message broken into three
				method( param, CreateString( 1232 ) );
				sent = prefix + CreateString( length) + "\r\n";
				sent += prefix + CreateString( length) + "\r\n";
				sent += prefix + CreateString( 1232 - ( length * 2) ) ;
				Assertion.AssertEquals( sent, BufferToString() );
			}
		}
		
		private void SingleColonParamTest( SingleColonParam method, string command, int length ) 
		{
			method("some text");
			Assertion.AssertEquals(command + " :some text", BufferToString() );
			try 
			{
				method(null );
				Assertion.Fail( "Null text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method( "" );
				Assertion.Fail( "Empty text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			
			method( CreateString( 600 ) );
			Assertion.AssertEquals( command + " :" + CreateString(length), BufferToString());	
		}


		private void SingleParamTest( SingleParam method, string command, string testValue ) 
		{
			method( testValue );
			Assertion.AssertEquals( command + " " + testValue, BufferToString() );
			try 
			{
				method(null);
				Assertion.Fail("Null");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("");
				Assertion.Fail("Empty");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("bad value" );
				Assertion.Fail("Bad value");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}			
			try 
			{
				method( CreateString(600) );
				Assertion.Fail("Long value");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}			
		}

		private void SingleOptionalParamTest( SingleOptionalParam method, string command, string[] testValues ) 
		{
			method( testValues[0] );
			Assertion.AssertEquals( command + " " + testValues[0], BufferToString() );

			method( testValues );
			Assertion.AssertEquals( command + " " + String.Join(",", testValues) , BufferToString() );

			try 
			{
				method(null);
				Assertion.Fail("Null");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("");
				Assertion.Fail("Empty");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method("bad value" );
				Assertion.Fail("Bad value");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}			
			try 
			{
				method( CreateString(600) );
				Assertion.Fail("Long value");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}			
		}


		[Test]
		public void TestQuitReason() 
		{
			SingleColonParamTest( new SingleColonParam( connection.Sender.Quit), "QUIT", 504 );
		}

		
	
		[Test]
		public void TestJoin() 
		{
			SingleParamTest( new SingleParam( connection.Sender.Join ), "JOIN", "#channel" );
		}

		[Test]
		public void TestJoinPassworded() 
		{
			DoubleParamTest( new DoubleParam( connection.Sender.Join ), "JOIN", "#channel", "password" );
		}

		[Test]
		public void TestNick()
		{
			SingleParamTest( new SingleParam( connection.Sender.Nick), "NICK","mynick");
		}

		[Test]
		public void TestNames() 
		{
			SingleOptionalParamTest( new SingleOptionalParam( connection.Sender.Names), "NAMES", new string[]{"#channel","#hello"});
		}

		[Test]
		public void TestAllNames() 
		{
			connection.Sender.AllNames();
			Assertion.AssertEquals( "NAMES", BufferToString() );
		}

		[Test]
		public void TestList() 
		{
			SingleOptionalParamTest( new SingleOptionalParam( connection.Sender.List), "LIST", new string[]{"#channel","#hello"});
		}

		[Test]
		public void TestAllList() 
		{
			connection.Sender.AllList();
			Assertion.AssertEquals( "LIST", BufferToString() );
		}

		[Test]
		public void TestChangeTopic() 
		{
			DoubleColonParamTest( new DoubleColonParam( connection.Sender.ChangeTopic ), "TOPIC", "#channel",495, true );
		}

		[Test]
		public void TestClearTopic() 
		{
			connection.Sender.ClearTopic( "#channel" );
			Assertion.AssertEquals("TOPIC #channel :", BufferToString() );
			try 
			{
				connection.Sender.ClearTopic("bad channel");
				Assertion.Fail( "Bad channel");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		[Test]
		public void TestRequestTopic() 
		{
			SingleParamTest( new SingleParam( connection.Sender.RequestTopic), "TOPIC","#channel" );			
		}

		[Test]
		public void TestPartMessage() 
		{
			connection.Sender.Part("bye", "#test");
			Assertion.AssertEquals("Part single","PART #test :bye", BufferToString() );

			connection.Sender.Part("bye", "#test","#test2");
			Assertion.AssertEquals("Part multiple","PART #test,#test2 :bye", BufferToString() );
		}

		[Test]
		public void TestPart() 
		{
			SingleParamTest( new SingleParam( connection.Sender.Part), "PART","#channel" );			
		}

		[Test]
		public void TestPublicNotice() 
		{
			DoubleColonParamTest( new DoubleColonParam( connection.Sender.PublicNotice ), "NOTICE", "#channel",493, false );
		}

		[Test]
		public void TestPrivateNotice() 
		{
			DoubleColonParamTest( new DoubleColonParam( connection.Sender.PrivateNotice ), "NOTICE", "mynick",495, false );
		}

		[Test]
		public void TestPublicMessage() 
		{
			DoubleColonParamTest( new DoubleColonParam( connection.Sender.PublicMessage ), "PRIVMSG", "#channel",493, false );
		}

		[Test]
		public void TestPrivateMessage() 
		{
			DoubleColonParamTest( new DoubleColonParam( connection.Sender.PrivateMessage ), "PRIVMSG", "mynick",495, false );
		}

		[Test]
		public void TestInvite() 
		{
			DoubleParamTest( new DoubleParam( connection.Sender.Invite ), "INVITE", "mynick","#channel");
		}

		[Test]
		public void TestKick() 
		{
			connection.Sender.Kick("#channel", "reason", "mynick");
			Assertion.AssertEquals( "KICK #channel mynick :reason", BufferToString() );

			connection.Sender.Kick("#channel", "reason", "alpha", "bravo" );
			Assertion.AssertEquals( "KICK #channel alpha,bravo :reason", BufferToString() );
			try 
			{
				connection.Sender.Kick(null, "reason","mynick");
				Assertion.Fail( "Null channel.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				connection.Sender.Kick("#channel", "reason",null);
				Assertion.Fail( "Null nick.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				connection.Sender.Kick("#channel", null, "mynick");
				Assertion.Fail( "Null reason.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			connection.Sender.Kick("#channel", CreateString( 600 ),"mynick" );
			Assertion.AssertEquals( "KICK #channel mynick :" + CreateString(488), BufferToString());		
		}

		[Test]
		public void TestIson() 
		{
			SingleParamTest( new SingleParam( connection.Sender.Ison), "ISON", "alpha" );		
		}

		[Test]
		public void TestWho() 
		{
			connection.Sender.Who("*.com", false );
			Assertion.AssertEquals( "WHO *.com", BufferToString() );
			connection.Sender.Who("*.com", true );
			Assertion.AssertEquals( "WHO *.com o", BufferToString() );
			try 
			{
				connection.Sender.Who(null, false);
				Assertion.Fail( "Null mask.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Who("", false);
				Assertion.Fail( "Empty mask.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Who(CreateString(600), false);
				Assertion.Fail( "Long mask.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		[Test]
		public void TestAllWho() 
		{
			connection.Sender.AllWho();
			Assertion.AssertEquals( "WHO", BufferToString() );
		}

		[Test]
		public void TestWhois() 
		{
			SingleParamTest( new SingleParam( connection.Sender.Whois), "WHOIS","mynick" );		
		}

		[Test]
		public void TestAway() 
		{
			SingleColonParamTest( new SingleColonParam( connection.Sender.Away), "AWAY", 504 );		
		}

		[Test]
		public void TestUnaway() 
		{
			connection.Sender.UnAway();
			Assertion.AssertEquals( "AWAY", BufferToString() );
		}

		[Test]
		public void TestWhowas() 
		{
			SingleParamTest( new SingleParam( connection.Sender.Whowas), "WHOWAS","mynick" );		
		}

		[Test]
		public void TestWhowasMax() 
		{
			connection.Sender.Whowas("mynick", 2 );
			Assertion.AssertEquals("WHOWAS mynick 2", BufferToString() );
			try 
			{
				connection.Sender.Whowas(null, 2 );
				Assertion.Fail( "Null first parameter.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Whowas("", 2 );
				Assertion.Fail( "Empty first parameter");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Whowas("my nick", 2 );
				Assertion.Fail( "Bad nick");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Whowas("mynick", 0 );
				Assertion.Fail( "Bad count");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}	
		}

		[Test]
		public void TestPong() 
		{
			connection.Sender.Pong(":112323213");
			Assertion.AssertEquals("PONG :112323213", BufferToString() );
		}

		[Test]
		public void TestPass() 
		{
			connection.Sender.Pass("*");
			Assertion.AssertEquals("PASS *", BufferToString() );
		}

		[Test]
		public void TestUser() 
		{
			ConnectionArgs args = new ConnectionArgs();
			args.UserName = "username";
			args.ModeMask = "i";
			args.RealName = "realname";
			connection.Sender.User( args );
			Assertion.AssertEquals("USER username i * realname", BufferToString() );
		}

		[Test]
		public void TestAction()
		{
			connection.Sender.Action("#sharktest","looks around");
			Assertion.AssertEquals( "PRIVMSG #sharktest :\x0001ACTION looks around\x0001", BufferToString() );
			try 
			{
				connection.Sender.Action(null, "a value");
				Assertion.Fail( "Null channel");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Action(null, null);
				Assertion.Fail( "Null channel and text.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				connection.Sender.Action("#sharktest", null );
				Assertion.Fail( "Null text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Action("#sharktest", "" );
				Assertion.Fail( "Empty text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Action("", "" );
				Assertion.Fail( "Empty command and text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.Action("", "some text" );
				Assertion.Fail( "Empty command");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			connection.Sender.Action( "#sharktest", CreateString( 600 ) );
			Assertion.AssertEquals("PRIVMSG #sharktest :\x0001ACTION " + CreateString(483) + "\x0001", BufferToString());
		}

		[Test]
		public void TestRequestUserModes()
		{
			connection.Sender.RequestUserModes();
			Assertion.AssertEquals( "MODE deltabot", BufferToString() );
		}

		[Test]
		public void TestChangeUserMode()
		{
			connection.Sender.ChangeUserMode( ModeAction.Add, UserMode.Invisible);
			Assertion.AssertEquals( "MODE deltabot +i", BufferToString() );
			try 
			{
				connection.Sender.ChangeUserMode(ModeAction.Add, UserMode.Away);
				Assertion.Fail( "Manual away set");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		[Test]
		public void TestRequestChannelMode() 
		{
			SingleParamTest( new SingleParam( connection.Sender.RequestChannelModes ), "MODE", "#channel" );
		}

		[Test]
		public void TestRequestChannelList() 
		{
			connection.Sender.RequestChannelList("#sharktest", ChannelMode.Ban );
			Assertion.AssertEquals( "MODE #sharktest b", BufferToString() );
			connection.Sender.RequestChannelList("#sharktest", ChannelMode.Exception );
			Assertion.AssertEquals( "MODE #sharktest e", BufferToString() );
			connection.Sender.RequestChannelList("#sharktest", ChannelMode.Invitation );
			Assertion.AssertEquals( "MODE #sharktest I", BufferToString() );
			connection.Sender.RequestChannelList("#sharktest", ChannelMode.ChannelCreator );
			Assertion.AssertEquals( "MODE #sharktest O", BufferToString() );

			try 
			{
				connection.Sender.RequestChannelList("", ChannelMode.Ban );
				Assertion.Fail( "Empty channel");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				connection.Sender.RequestChannelList("#sharktest", ChannelMode.Anonymous );
				Assertion.Fail( "Wrong channel mode" );
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		[Test]
		public void TestChangeChannelMode() 
		{
			connection.Sender.ChangeChannelMode( "#sharktest", ModeAction.Add, ChannelMode.UserLimit, "10" );
			Assertion.AssertEquals( "MODE #sharktest +l 10", BufferToString() );

			connection.Sender.ChangeChannelMode( "#sharktest", ModeAction.Remove, ChannelMode.Ban, "Jocko!*@*" );
			Assertion.AssertEquals( "MODE #sharktest -b Jocko!*@*", BufferToString() );

			connection.Sender.ChangeChannelMode( "#sharktest", ModeAction.Add, ChannelMode.Private, null );
			Assertion.AssertEquals( "MODE #sharktest +p", BufferToString() );

			try 
			{
				connection.Sender.ChangeChannelMode("", ModeAction.Add, ChannelMode.Moderated, null );
				Assertion.Fail( "Empty channel");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		[Test]
		public void TestPrivateAction()
		{
			connection.Sender.PrivateAction("nick","looks around");
			Assertion.AssertEquals( "PRIVMSG nick :\x0001ACTION looks around\x0001", BufferToString() );
			try 
			{
				connection.Sender.PrivateAction(null, "a value");
				Assertion.Fail( "Null nick");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.PrivateAction(null, null);
				Assertion.Fail( "Null nick and text.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				connection.Sender.PrivateAction("nick", null );
				Assertion.Fail( "Null text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.PrivateAction("nick", "" );
				Assertion.Fail( "Empty text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.PrivateAction("", "" );
				Assertion.Fail( "Empty nick and text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				connection.Sender.PrivateAction("", "some text" );
				Assertion.Fail( "Empty nick");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			connection.Sender.PrivateAction( "abcdej", CreateString( 600 ) );
			Assertion.AssertEquals("PRIVMSG abcdej :\x0001ACTION " + CreateString(487) + "\x0001", BufferToString());
		}

		[Test]
		public void TestRaw()
		{
			connection.Sender.Raw("raw message");
			Assertion.AssertEquals( "raw message", BufferToString() );
			try 
			{
				connection.Sender.Raw(null );
				Assertion.Fail( "Null message");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			
			try 
			{
				connection.Sender.Raw( "" );
				Assertion.Fail( "Empty message");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			connection.Sender.Raw( CreateString( 600 ) );
			Assertion.AssertEquals( CreateString(512), BufferToString());
		}

		[Test]
		public void TestVersion()
		{
			connection.Sender.Version();
			Assertion.AssertEquals( "VERSION", BufferToString() );
			
			connection.Sender.Version(" ");
			Assertion.AssertEquals( "VERSION", BufferToString() );

			connection.Sender.Version("*.server.com");
			Assertion.AssertEquals( "VERSION *.server.com", BufferToString() );
			
			connection.Sender.Version( CreateString( 600 ) );
			Assertion.AssertEquals("VERSION " + CreateString(502) , BufferToString());
		}

		[Test]
		public void TestMotd()
		{
			connection.Sender.Motd();
			Assertion.AssertEquals( "MOTD", BufferToString() );
			
			connection.Sender.Motd(" ");
			Assertion.AssertEquals( "MOTD", BufferToString() );

			connection.Sender.Motd("*.server.com");
			Assertion.AssertEquals( "MOTD *.server.com", BufferToString() );
			
			connection.Sender.Motd( CreateString( 600 ) );
			Assertion.AssertEquals("MOTD " + CreateString(505) , BufferToString());
		}

		[Test]
		public void TestTime()
		{
			connection.Sender.Time();
			Assertion.AssertEquals( "TIME", BufferToString() );
			
			connection.Sender.Time(" ");
			Assertion.AssertEquals( "TIME", BufferToString() );

			connection.Sender.Time("*.server.com");
			Assertion.AssertEquals( "TIME *.server.com", BufferToString() );
			
			connection.Sender.Time( CreateString( 600 ) );
			Assertion.AssertEquals("TIME " + CreateString(504) , BufferToString());
		}

		[Test]
		public void TestWallops()
		{
			
			try 
			{
				connection.Sender.Wallops(" ");
				Assertion.Fail();
			}
			catch( Exception e ) 
			{
				Assertion.Assert("Wallops: empty message", true );
			}

			connection.Sender.Wallops("I spam you!");
			Assertion.AssertEquals( "WALLOPS I spam you!", BufferToString() );
			
			connection.Sender.Wallops( CreateString( 600 ) );
			Assertion.AssertEquals("WALLOPS " + CreateString(502) , BufferToString());
		}

		[Test]
		public void TestInfo()
		{
			connection.Sender.Info();
			Assertion.AssertEquals( "INFO", BufferToString() );
			
			connection.Sender.Info(" ");
			Assertion.AssertEquals( "INFO", BufferToString() );

			connection.Sender.Info("*.server.com");
			Assertion.AssertEquals( "INFO *.server.com", BufferToString() );

			connection.Sender.Info("bob");
			Assertion.AssertEquals( "INFO bob", BufferToString() );
			
			connection.Sender.Info( CreateString( 600 ) );
			Assertion.AssertEquals("INFO " + CreateString(505) , BufferToString());
		}

		[Test]
		public void TestAdmin()
		{
			connection.Sender.Admin();
			Assertion.AssertEquals( "ADMIN", BufferToString() );
			
			connection.Sender.Admin(" ");
			Assertion.AssertEquals( "ADMIN", BufferToString() );

			connection.Sender.Admin("*.server.com");
			Assertion.AssertEquals( "ADMIN *.server.com", BufferToString() );

			connection.Sender.Admin("bob");
			Assertion.AssertEquals( "ADMIN bob", BufferToString() );
			
			connection.Sender.Admin( CreateString( 600 ) );
			Assertion.AssertEquals("ADMIN " + CreateString(504) , BufferToString());
		}

		[Test]
		public void TestLusers()
		{
			connection.Sender.Lusers();
			Assertion.AssertEquals( "LUSERS", BufferToString() );

			connection.Sender.Lusers(null,null);
			Assertion.AssertEquals( "LUSERS", BufferToString() );
			
			connection.Sender.Lusers(null,"irc.gamesnet.net");
			Assertion.AssertEquals( "LUSERS irc.gamesnet.net", BufferToString() );

			connection.Sender.Lusers("*.server.com","irc.gamesnet.net");
			Assertion.AssertEquals( "LUSERS *.server.com irc.gamesnet.net", BufferToString() );

			try 
			{
				connection.Sender.Lusers(CreateString( 300 ), CreateString( 300 ));
				Assertion.Fail();
			}
			catch( Exception e ) 
			{
				Assertion.Assert("Lusers: too long message", true );
			}
		}

		[Test]
		public void TestLinks()
		{
			connection.Sender.Links();
			Assertion.AssertEquals( "LINKS", BufferToString() );

			connection.Sender.Links("*.edu" );
			Assertion.AssertEquals( "LINKS *.edu", BufferToString() );
			
			connection.Sender.Links("*.edu","irc.gamesnet.net");
			Assertion.AssertEquals( "LINKS *.edu irc.gamesnet.net", BufferToString() );

			try 
			{
				connection.Sender.Links(CreateString( 300 ), CreateString( 300 ));
				Assertion.Fail();
			}
			catch( Exception e ) 
			{
				Assertion.Assert("Links: too long message", true );
			}
		}

		[Test]
		public void TestStats()
		{
			connection.Sender.Stats( StatsQuery.CommandUsage );
			Assertion.AssertEquals( "STATS m", BufferToString() );

			connection.Sender.Stats( StatsQuery.Connections );
			Assertion.AssertEquals( "STATS l", BufferToString() );

			connection.Sender.Stats( StatsQuery.Operators );
			Assertion.AssertEquals( "STATS o", BufferToString() );
			
			connection.Sender.Stats( StatsQuery.Uptime );
			Assertion.AssertEquals( "STATS u", BufferToString() );

			connection.Sender.Stats( StatsQuery.Uptime, "irc.gnome.org" );
			Assertion.AssertEquals( "STATS u irc.gnome.org", BufferToString() );

			try 
			{
				connection.Sender.Stats( StatsQuery.CommandUsage, CreateString( 600 ));
				Assertion.Fail();
			}
			catch( Exception e ) 
			{
				Assertion.Assert("Stats: too long message", true );
			}
		}

	}
}
#endif