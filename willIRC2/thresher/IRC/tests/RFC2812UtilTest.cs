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
	/// Test Rfc2812Util
	/// </summary>
	[TestFixture]
	public class Rfc2812UtilTest 
	{

		string longChannel;

		[SetUp]	
		public void SetUp() 
		{ 
			longChannel = "";
			for( int i = 0; i < 51; i++ ) 
			{
				longChannel += "a";
			}
		}

		[Test]
		public void TestIsValidChannelName() 
		{
			Assertion.Assert( "Good: #", Rfc2812Util.IsValidChannelName( "#normal" ) );
			Assertion.Assert( "Good: +", Rfc2812Util.IsValidChannelName( "+normal" ) );
			Assertion.Assert( "Bad :char", !Rfc2812Util.IsValidChannelName( "=channel" ) );
			Assertion.Assert( "Bad: space", !Rfc2812Util.IsValidChannelName( "#space channel" ) );
			Assertion.Assert( "Bad: length", !Rfc2812Util.IsValidChannelName( longChannel ) );
			Assertion.Assert( "Bad: null", !Rfc2812Util.IsValidChannelName( null ) );
			Assertion.Assert( "Bad: empty", !Rfc2812Util.IsValidChannelName( "" ) );
		}

		[Test]
		public void TestIsValidChannelList() 
		{
			Assertion.Assert( "Good:", Rfc2812Util.IsValidChannelList( new string[]{"#normal","#hello"} ) );
			Assertion.Assert( "Bad 1 is empty:", !Rfc2812Util.IsValidChannelList( new string[]{"#normal",""} ) );
			Assertion.Assert( "Bad 2 are invalid:", !Rfc2812Util.IsValidChannelList( new string[]{"normal","hello"} ) );
			Assertion.Assert( "Bad 1 is null:", !Rfc2812Util.IsValidChannelList( new string[]{"#normal",null} ) );

		}

		[Test]
		public void TestIsValidNick() 
		{
			Connection connection = new Connection( new ConnectionArgs("test","dummy"), false,false );
			Assertion.Assert( "Good", Rfc2812Util.IsValidNick( "alpha" ) );
			Assertion.Assert( "Good: chars1", Rfc2812Util.IsValidNick( "[BAD]_o" ) );
			Assertion.Assert( "Good: chars2", Rfc2812Util.IsValidNick( "{BAD}`o|" ) );
			Assertion.Assert( "Bad: null", !Rfc2812Util.IsValidNick( null ) );
			Assertion.Assert( "Bad: empty", !Rfc2812Util.IsValidNick( "" ) );
			Assertion.Assert( "Bad: space", !Rfc2812Util.IsValidNick( "aa aa" ) );
		}

		[Test]
		public void TestIsValidNickList() 
		{
			Connection connection = new Connection( new ConnectionArgs("test","dummy"), false,false );
			Assertion.Assert( "Good:", Rfc2812Util.IsValidNicklList( new string[]{"bob"} ) );
			Assertion.Assert( "Bad 1 is empty:", !Rfc2812Util.IsValidNicklList( new string[]{"bob",""} ) );
			Assertion.Assert( "Bad 2 are invalid:", !Rfc2812Util.IsValidNicklList( new string[]{"bad nick",""} ) );
			Assertion.Assert( "Bad 1 is null:", !Rfc2812Util.IsValidNicklList( new string[]{"bob",null} ) );
		}

		[Test]
		public void TestModeActionToChar() 
		{
			Assertion.AssertEquals("Good: add", '+', Rfc2812Util.ModeActionToChar( ModeAction.Add ) );
			Assertion.AssertEquals("Good: remove", '-', Rfc2812Util.ModeActionToChar( ModeAction.Remove ) );
		}

		[Test]
		public void TestCharToModeAction() 
		{
			Assertion.AssertEquals("Good: add", ModeAction.Add, Rfc2812Util.CharToModeAction( '+' ));
			Assertion.AssertEquals("Good: remove", ModeAction.Remove, Rfc2812Util.CharToModeAction( '-' ) );
		}

		[Test]
		public void TestUserModeToChar() 
		{
			Assertion.AssertEquals("Good: away", 'a', Rfc2812Util.UserModeToChar( UserMode.Away ) );
			Assertion.AssertEquals("Good: invisible", 'i', Rfc2812Util.UserModeToChar( UserMode.Invisible ) );
			Assertion.AssertEquals("Good: local op", 'O', Rfc2812Util.UserModeToChar( UserMode.LocalOperator ) );
			Assertion.AssertEquals("Good: op", 'o', Rfc2812Util.UserModeToChar( UserMode.Operator ) );
			Assertion.AssertEquals("Good: restricted", 'r', Rfc2812Util.UserModeToChar( UserMode.Restricted ) );
			Assertion.AssertEquals("Good: wallops", 'w', Rfc2812Util.UserModeToChar( UserMode.Wallops ) );
		}

		[Test]
		public void TestCharToUserMode() 
		{
			Assertion.AssertEquals("Good: away", UserMode.Away, Rfc2812Util.CharToUserMode( 'a') );
			Assertion.AssertEquals("Good: invisible", UserMode.Invisible, Rfc2812Util.CharToUserMode( 'i' ) );
			Assertion.AssertEquals("Good: local op", UserMode.LocalOperator, Rfc2812Util.CharToUserMode( 'O' ) );
			Assertion.AssertEquals("Good: op", UserMode.Operator, Rfc2812Util.CharToUserMode( 'o' ) );
			Assertion.AssertEquals("Good: restricted", UserMode.Restricted, Rfc2812Util.CharToUserMode( 'r' ) );
			Assertion.AssertEquals("Good: wallops", UserMode.Wallops, Rfc2812Util.CharToUserMode( 'w' ) );
		}

		[Test]
		public void TestCharToChannelMode() 
		{
			Assertion.AssertEquals("Good: ChannelCreator", ChannelMode.ChannelCreator, Rfc2812Util.CharToChannelMode( 'O'));
			Assertion.AssertEquals("Good: ChannelOperator", ChannelMode.ChannelOperator, Rfc2812Util.CharToChannelMode( 'o'));
			Assertion.AssertEquals("Good: Voice", ChannelMode.Voice, Rfc2812Util.CharToChannelMode( 'v'));
			Assertion.AssertEquals("Good: Anonymous ", ChannelMode.Anonymous, Rfc2812Util.CharToChannelMode( 'a'));
			Assertion.AssertEquals("Good: InviteOnly", ChannelMode.InviteOnly, Rfc2812Util.CharToChannelMode( 'i'));
			Assertion.AssertEquals("Good: Moderated", ChannelMode.Moderated, Rfc2812Util.CharToChannelMode( 'm'));
			Assertion.AssertEquals("Good: NoOutside", ChannelMode.NoOutside, Rfc2812Util.CharToChannelMode( 'n'));
			Assertion.AssertEquals("Good: Quiet", ChannelMode.Quiet, Rfc2812Util.CharToChannelMode( 'q'));
			Assertion.AssertEquals("Good: Private", ChannelMode.Private, Rfc2812Util.CharToChannelMode( 'p'));
			Assertion.AssertEquals("Good: Secret", ChannelMode.Secret, Rfc2812Util.CharToChannelMode( 's'));
			Assertion.AssertEquals("Good: ServerReop", ChannelMode.ServerReop, Rfc2812Util.CharToChannelMode( 'r'));
			Assertion.AssertEquals("Good: TopicSettable", ChannelMode.TopicSettable, Rfc2812Util.CharToChannelMode( 't'));
			Assertion.AssertEquals("Good: Password", ChannelMode.Password, Rfc2812Util.CharToChannelMode( 'k'));
			Assertion.AssertEquals("Good: UserLimit", ChannelMode.UserLimit, Rfc2812Util.CharToChannelMode( 'l'));
			Assertion.AssertEquals("Good: Ban", ChannelMode.Ban, Rfc2812Util.CharToChannelMode( 'b'));
			Assertion.AssertEquals("Good: Exception", ChannelMode.Exception, Rfc2812Util.CharToChannelMode( 'e'));
			Assertion.AssertEquals("Good: Invitation", ChannelMode.Invitation, Rfc2812Util.CharToChannelMode( 'I'));
		}

		[Test]
		public void TestChannelModeToChar() 
		{
			Assertion.AssertEquals("Good: ChannelCreator", 'O', Rfc2812Util.ChannelModeToChar(ChannelMode.ChannelCreator ));
			Assertion.AssertEquals("Good: ChannelOperator", 'o', Rfc2812Util.ChannelModeToChar(ChannelMode.ChannelOperator ));
			Assertion.AssertEquals("Good: Voice", 'v' , Rfc2812Util.ChannelModeToChar( ChannelMode.Voice));
			Assertion.AssertEquals("Good: Anonymous ", 'a' , Rfc2812Util.ChannelModeToChar(ChannelMode.Anonymous));
			Assertion.AssertEquals("Good: InviteOnly", 'i' , Rfc2812Util.ChannelModeToChar(ChannelMode.InviteOnly));
			Assertion.AssertEquals("Good: Moderated", 'm', Rfc2812Util.ChannelModeToChar(ChannelMode.Moderated ));
			Assertion.AssertEquals("Good: NoOutside", 'n' , Rfc2812Util.ChannelModeToChar(ChannelMode.NoOutside));
			Assertion.AssertEquals("Good: Quiet", 'q' , Rfc2812Util.ChannelModeToChar(ChannelMode.Quiet));
			Assertion.AssertEquals("Good: Private", 'p' , Rfc2812Util.ChannelModeToChar(ChannelMode.Private));
			Assertion.AssertEquals("Good: Secret", 's' , Rfc2812Util.ChannelModeToChar(ChannelMode.Secret));
			Assertion.AssertEquals("Good: ServerReop", 'r' , Rfc2812Util.ChannelModeToChar(ChannelMode.ServerReop));
			Assertion.AssertEquals("Good: TopicSettable", 't', Rfc2812Util.ChannelModeToChar(ChannelMode.TopicSettable));
			Assertion.AssertEquals("Good: Password", 'k', Rfc2812Util.ChannelModeToChar(ChannelMode.Password ));
			Assertion.AssertEquals("Good: UserLimit", 'l' , Rfc2812Util.ChannelModeToChar(ChannelMode.UserLimit));
			Assertion.AssertEquals("Good: Ban", 'b', Rfc2812Util.ChannelModeToChar(ChannelMode.Ban));
			Assertion.AssertEquals("Good: Exception", 'e' , Rfc2812Util.ChannelModeToChar(ChannelMode.Exception));
			Assertion.AssertEquals("Good: Invitation", 'I', Rfc2812Util.ChannelModeToChar(ChannelMode.Invitation));
		}

		[Test]
		public void TestUserInfoFromString() 
		{
			UserInfo info = Rfc2812Util.UserInfoFromString( "Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net" );
			Assertion.AssertEquals("UserInfo.Nick","Scurvy", info.Nick );
			Assertion.AssertEquals("UserInfo.User","~Scurvy", info.User );
			Assertion.AssertEquals("UserInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", info.Hostname );
			
			info = Rfc2812Util.UserInfoFromString( "Scurvy");
			Assertion.AssertEquals("UserInfo.Nick","Scurvy", info.Nick );
			Assertion.AssertEquals("UserInfo.User","", info.User );
			Assertion.AssertEquals("UserInfo.Host","", info.Hostname );

			info = Rfc2812Util.UserInfoFromString( "");
			Assertion.AssertEquals("Empty", UserInfo.Empty, info );

		}

		[Test]
		public void TestParseUserInfoLine() 
		{
			string[] parts = Rfc2812Util.ParseUserInfoLine( "Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net" );
			Assertion.AssertEquals("UserInfo.Nick","Scurvy", parts[0] );
			Assertion.AssertEquals("UserInfo.User","~Scurvy", parts[1]);
			Assertion.AssertEquals("UserInfo.Host","pcp825822pcs.nrockv01.md.comcast.net",parts[2] );
			
			parts = Rfc2812Util.ParseUserInfoLine( "Scurvy");
			Assertion.AssertEquals("UserInfo.Nick","Scurvy",parts[0] );
			Assertion.AssertEquals("UserInfo.User","", parts[1] );
			Assertion.AssertEquals("UserInfo.Host","", parts[2]);

			parts = Rfc2812Util.ParseUserInfoLine( "");
			Assertion.AssertNull("Empty", parts );

		}

	}
}
#endif
