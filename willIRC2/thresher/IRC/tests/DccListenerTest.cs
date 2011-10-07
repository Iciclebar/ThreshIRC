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
using System.Net;
using Sharkbite.Irc;
using NUnit.Framework;


namespace Sharkbite.Irc.Test
{
	/// <summary>
	/// Summary description for DccListenerTest.
	/// </summary>
	[TestFixture]
	public class DccListenerTest 
	{
		private string[] testLines;
		private Connection connection;

		[SetUp]
		public void SetUp() 
		{
			ConnectionArgs args = new ConnectionArgs("deltabot","irc.sventech.com");
			connection = new Connection( args );
			RegisterListeners();
			AssignLines();
		}

		private void RegisterListeners() 
		{
			DccListener.DefaultInstance.OnDccChatRequest += new DccChatRequestEventHandler( OnDccChatRequest );
			connection.Listener.OnError += new ErrorMessageEventHandler( OnError );
		}

		private void AssignLines() 
		{
			testLines = new string[]{
				":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG bot :\x0001DCC CHAT CHAT 3232235531 46000\x0001" //Good request
			};
		}

		[Test]
		public void TestIsDccRequest() 
		{
			string chat = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG bot :\x0001DCC CHAT chat 3232235531 46000\x0001";
			Assertion.Assert("Sucess: chat request", DccListener.IsDccRequest( chat ) );

			chat = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG bot :\x0001DCC VOICE 3232235531 46000\x0001";
			Assertion.Assert("Fail: Unsupported DCC type", !DccListener.IsDccRequest( chat ) );

			string unquoteChat = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net PRIVMSG bot :\x0001DCC CHAT chat 13246 46000";
			Assertion.Assert("Fail: unqutoted chat request", !DccListener.IsDccRequest( unquoteChat ) );

			string finger = ":Scurvy!~Scurvy@pcp825822pcs.nrockv01.md.comcast.net NOTICE alphaX234 :\u0001FINGER Scurvy. Idle time is 12secs.\u0001";
			Assertion.Assert("Fail: Non DCC request", !DccListener.IsDccRequest( finger ) );
		}

		[Test]
		public void TestParse() 
		{
			foreach( string line in testLines ) 
			{
				DccListener.DefaultInstance.Parse(connection, line );
			}
		}

		public void OnDccChatRequest(DccUserInfo dccUserInfo ) 
		{
			//Test for protocol too
			Assertion.AssertEquals("OnDccChatRequest: Hostname", "irc.sventech.com", dccUserInfo.Connection.connectionArgs.Hostname );
			Assertion.AssertEquals("OnDccChatRequest: userInfo.Nick","Scurvy", dccUserInfo.Nick );
			Assertion.AssertEquals("OnDccChatRequest: userInfo.User","~Scurvy", dccUserInfo.User );
			Assertion.AssertEquals("OnDccChatRequest: userInfo.Host","pcp825822pcs.nrockv01.md.comcast.net", dccUserInfo.Hostname );			
			Assertion.AssertEquals("OnDccChatRequest: end point", "192.168.0.11:46000", dccUserInfo.RemoteEndPoint.ToString() );	
			Console.WriteLine("OnDccChatRequest");

		}

		public void OnError( ReplyCode code, string message ) 
		{
			Console.WriteLine("OnError:" + message );
		}

	}
}
#endif