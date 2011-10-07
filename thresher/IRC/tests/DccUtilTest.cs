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
	/// Test DccUtilTest.
	/// </summary>
	[TestFixture]
	public class DccUtilTest
	{

		private string quadOne;
		private string networkOne;

		private string quadTwo;
		private string networkTwo;

		[SetUp]
		public void SetUp() 
		{
			networkOne = "3232235531"; 
			quadOne = "192.168.0.11";

			networkTwo = "1091311908";
			quadTwo = "65.12.25.36";

		}

		[Test]
		public void TestIpAddressToLong() 
		{
			IPAddress ip = IPAddress.Parse( quadOne );
			string network = DccUtil.IPAddressToLong( ip );
			Assertion.AssertEquals(networkOne, network );

			ip = IPAddress.Parse( quadTwo );
			network = DccUtil.IPAddressToLong( ip );
			Assertion.AssertEquals(networkTwo, network );

			try 
			{
				network = DccUtil.IPAddressToLong( null );
				Assertion.Fail();
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

		[Test]
		public void TestLongToIpAddress() 
		{
			IPAddress ip = DccUtil.LongToIPAddress( networkOne );
			Assertion.AssertEquals(quadOne, ip.ToString() );

			ip = DccUtil.LongToIPAddress( networkTwo );
			Assertion.AssertEquals(quadTwo, ip.ToString() );

			try 
			{
				ip = DccUtil.LongToIPAddress( null );
				Assertion.Fail();
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}

			try 
			{
				ip = DccUtil.LongToIPAddress( "akak" );
				Assertion.Fail();
			}
			catch( ArgumentException ae ) 
			{
				Assertion.Assert( true );
			}
		}

	

		[Test]
		public void TestDccBytesReceivedFormat() 
		{
			long a = 31240;
			byte[] aBytes = DccUtil.DccBytesReceivedFormat( a );
			Assertion.Assert( aBytes.Length == 4 );
			Assertion.Assert( aBytes[0] == 0 );
			Assertion.Assert( aBytes[1] == 0);
			Assertion.Assert( aBytes[2] == 122);
			Assertion.Assert( aBytes[3] == 8);

			long b= 120000;
			byte[] bBytes = DccUtil.DccBytesReceivedFormat( b );
			Assertion.Assert( bBytes.Length == 4 );
			Assertion.Assert( bBytes[0] == 0 );
			Assertion.Assert( bBytes[1] == 1 );
			Assertion.Assert( bBytes[2] == 212 );
			Assertion.Assert( bBytes[3] == 192);
		}

		

	}
}
#endif