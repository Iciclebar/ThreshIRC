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
	public class TextColorTest
	{


		[Test]
		public void TestStripControlChars() 
		{
			string text = "\x0002\x00034Hello\x0003\x0002";
			Assertion.AssertEquals("TextColorTest: Strip1", "Hello", TextColor.StripControlChars( text ) );

			text = "\x0002Hello\x0002";
			Assertion.AssertEquals("TextColorTest: Strip2", "Hello", TextColor.StripControlChars( text ) );

			text = "\x0002\x000315Hello\x0003\x0002";
			Assertion.AssertEquals("TextColorTest: Strip3", "Hello", TextColor.StripControlChars( text ) );

			text = "\x0002\x0003Hello\x0003\x0002";
			Assertion.AssertEquals("TextColorTest: Strip4", "Hello", TextColor.StripControlChars( text ) );

			text = "\x00032,1Hello\x0003";
			Assertion.AssertEquals("TextColorTest: Text & Background", "Hello", TextColor.StripControlChars( text ) );

			text = "\x000345Hello\x0003";
			Assertion.AssertEquals("TextColorTest: Large color number", "Hello", TextColor.StripControlChars( text ) );
		}

		[Test]
		public void TestMakeBold() 
		{
			string text = "\x02Hello\x02";
			Assertion.AssertEquals("TextColorTest: Make Bold", text, TextColor.MakeBold( "Hello" ) );
		}

		[Test]
		public void TestMakePlain() 
		{
			string text = "\x0FHello\x0F";
			Assertion.AssertEquals("TextColorTest: Make Plain", text, TextColor.MakePlain( "Hello" ) );
		}

		[Test]
		public void TestMakeUnderline() 
		{
			string text = "\x1FHello\x1F";
			Assertion.AssertEquals("TextColorTest: Make Underline", text, TextColor.MakeUnderline( "Hello" ) );
		}

		[Test]
		public void TestMakeReverseVideo() 
		{
			string text = "\x16Hello\x16";
			Assertion.AssertEquals("TextColorTest: Make Reverse", text, TextColor.MakeReverseVideo( "Hello" ) );
		}

		[Test]
		public void TestMakeColor() 
		{
			string text = "\x03" +  "2Hello\x03";
			Assertion.AssertEquals("TextColorTest: Make Color", text, TextColor.MakeColor( "Hello", MircColor.Blue ) );
		}

		[Test]
		public void TestFullMakeColor() 
		{
			string text = "\x00032,1Hello\x0003";
			Assertion.AssertEquals("FullColorTest: Make Color", text, TextColor.MakeColor( "Hello", MircColor.Blue, MircColor.Black ) );
		}

		[Test]
		public void TestComposite() 
		{
			string text = "\x02\x03" + "2Hello\x03\x02";
			Assertion.AssertEquals("TextColorTest: Make Composite", text, 
				TextColor.MakeBold( TextColor.MakeColor( "Hello", MircColor.Blue ) ) );
		}

	
	}
}
#endif