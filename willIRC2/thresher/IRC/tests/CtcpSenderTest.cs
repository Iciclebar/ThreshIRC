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
	/// Summary description for CtcpSenderTest.
	/// </summary>
	[TestFixture]
	public class CtcpSenderTest
	{
		/// <summary>
		/// A delegate that wraps mosts Ctcp requests
		/// </summary>
		private delegate void RequestParam( string command, string nick );

		/// <summary>
		/// A delegate that wraps mosts Ctcp replies
		/// </summary>
		private delegate void ReplyParam( string command, string nick, string text );

		private const char Quote = '\x0001';
		private MemoryStream buffer;
		private Connection connection;


		[SetUp]
		public void SetUp() 
		{ 
			ConnectionArgs args = new ConnectionArgs("deltabot","irc.sventech.com");
			args.Hostname = "irc.sventech.com";
			buffer = new MemoryStream();
			connection = new Connection( args );
			connection.writer = new StreamWriter( buffer );
			connection.writer.AutoFlush = true;
		}

		private void RequestParamTest( RequestParam method, string command ) 
		{
			method( command, "mynick" );
			Assertion.AssertEquals( "PRIVMSG mynick :" + Quote + command + Quote , BufferToString() );
			try 
			{
				method(command, null);
				Assertion.Fail("Null");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command,"");
				Assertion.Fail("Empty");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command,"bad value" );
				Assertion.Fail("Bad value");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}			
			try 
			{
				method(null,"mynick" );
				Assertion.Fail("Bad command");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}		
		}

		private void ReplyParamTest( ReplyParam method, string command, int maxLength ) 
		{
			method(command, "mynick","alpha bravo! charlie. DELTA99");
			Assertion.AssertEquals( "NOTICE mynick :\x0001" + command + " alpha bravo! charlie. DELTA99\x0001", BufferToString() );
			try 
			{
				method(null, "nick", "a value");
				Assertion.Fail( "Null command");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command, null, "a value");
				Assertion.Fail( "Null nick");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command, null, null);
				Assertion.Fail( "Null nick and text.");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				method(command, "mynick", null );
				Assertion.Fail( "Null text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command, "mynick", "" );
				Assertion.Fail( "Empty text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command, "", "" );
				Assertion.Fail( "Empty nick and text");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			try 
			{
				method(command, "", "some text" );
				Assertion.Fail( "Empty nick");
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
			method( command, "mynick", CreateString( 600 ) );
			int length = 512 - "mynick".Length - maxLength;
			Assertion.AssertEquals("NOTICE mynick :\x0001" + command + " " + CreateString(length) + "\x0001", BufferToString() );
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

		private string CreateString( int length ) 
		{
			StringBuilder builder = new StringBuilder( length );
			for( int i = 0; i < length; i ++ ) 
			{
				builder.Append("X");
			}
			return builder.ToString();
		}
	
	
		[Test]
		public void TestCtcpRequest() 
		{
			RequestParamTest( new RequestParam( connection.CtcpSender.CtcpRequest ), "FINGER" );
		}

		[Test]
		public void TestCtcpReply() 
		{
			ReplyParamTest( new ReplyParam( connection.CtcpSender.CtcpReply ), "FINGER", 20 );
		}

		
	}
}
#endif