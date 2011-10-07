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
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;


namespace Sharkbite.Irc.Test
{
	/// <summary>
	/// Summary description for IdentdTest.
	/// </summary>
	[TestFixture]
	public class IdentdTest 
	{

		[Test]
		public void TestStart() 
		{
			Identd.Start("username");
			Thread.Sleep( 1000 );
			Assertion.Assert( Identd.IsRunning() );

			try 
			{
				Identd.Start("username");
				Assertion.Fail();
			}
			catch( Exception e) 
			{
				Assertion.Assert("Restart failed", true );
			}

			Identd.Stop();
		}

		[Test]
		public void TestStop() 
		{
			Identd.Start("username");
			Thread.Sleep( 1000 );
			Identd.Stop();
			Assertion.Assert( !Identd.IsRunning() );
		}

		[Test]
		public void TestListen() 
		{
			try 
			{
				Identd.Start("username");
				Thread.Sleep( 1000 );

				TcpClient client = new TcpClient();
				client.Connect("localhost", 113);

				StreamWriter writer = new StreamWriter( client.GetStream() );
				writer.WriteLine( "a query" );
				writer.Flush();

				StreamReader reader = new StreamReader( client.GetStream() );
				string line = reader.ReadLine();

				Identd.Stop();

				Assertion.AssertEquals( "a query : USERID : UNIX : username", line.Trim() );
			}
			catch( Exception e ) 
			{
				Assertion.Fail("IO Exception during test:" + e);
			}
		}

	}
}
#endif
