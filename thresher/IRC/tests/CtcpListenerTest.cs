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
	/// Summary description for listenerTest.
	/// </summary>
	[TestFixture]
	public class CtcpListenerTest
	{

		private string[] testLines;
		private CtcpListener listener;
		private Connection connection;

		
		[SetUp]
		public void SetUp() 
		{ 
			ConnectionArgs args = new ConnectionArgs("deltabot","irc.sventech.com");
			connection = new Connection( args );
			listener = new CtcpListener( connection);
			RegisterListeners();
			AssignLines();
		}

		private void RegisterListeners() 
		{
			listener.OnCtcpReply += new CtcpReplyEventHandler( OnCtcpReply );
			listener.OnCtcpRequest += new CtcpRequestEventHandler( OnCtcpRequest );
			}

		private void AssignLines() 
		{
			testLines = new string[]{
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG bot :\x0001FINGER\x0001", //OnFingerRequest
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net NOTICE bot :\x0001FINGER Hello. Idle time is 10s.\x0001" //OnFingerReply
									};
		}

		[Test]
		public void TestIsCtcpMessage() 
		{
			string finger = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net NOTICE alphaX234 :\u0001FINGER Scurvy. Idle time is 12secs.\u0001";
			Assertion.Assert( "Success: finger", CtcpListener.IsCtcpMessage( finger ) );

			string action = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG #sharktest :\u0001ACTION Looks around\u0001";
			Assertion.Assert( "Fail: action", !CtcpListener.IsCtcpMessage( action ) );
			
			string privmsg = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG alphaX234 :test of private message";
			Assertion.Assert( "Fail: privmsg", !CtcpListener.IsCtcpMessage( privmsg ) );

			string source = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG alphaX234 :\u0001SOURCE\u0001";
			Assertion.Assert( "Success: source", CtcpListener.IsCtcpMessage( source ) );
		}

		[Test]
		public void TestParse() 
		{
			foreach( string line in testLines ) 
			{
				CtcpListener.IsCtcpMessage( line );
				listener.Parse( line );
			}
		}

		public void OnCtcpRequest( string command, UserInfo who ) 
		{
			Assertion.AssertEquals( "OnCtcpRequest: command", "FINGER", command);
			Assertion.AssertEquals("OnCtcpRequest: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnCtcpRequest: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnCtcpRequest: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Console.WriteLine("OnCtcpRequest");
		}

		public void OnCtcpReply( string command, UserInfo who, string text ) 
		{
			Assertion.AssertEquals( "OnCtcpReply: command", "FINGER", command);
			Assertion.AssertEquals("OnCtcpReply: userInfo.Nick","Scurvy", who.Nick );
			Assertion.AssertEquals("OnCtcpReply: userInfo.User","~Scurvy", who.User );
			Assertion.AssertEquals("OnCtcpReply: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", who.Hostname );
			Assertion.AssertEquals( "OnCtcpReply: text", "Hello. Idle time is 10s.", text);
			Console.WriteLine("OnCtcpReply");
		}

	
	}
}
#endif