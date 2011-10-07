/*
  * Copyright (c) 2002 Aaron Hunter
  * All rights reserved.
  *
  * Redistribution and use in source and binary forms, with or without
  * modification, are permitted provided that the following conditions
  * are met:
  *
  * 1. Redistributions of source code must retain the above copyright
  *     notice, this list of conditions and the following disclaimer.
  * 2. Redistributions in binary form must reproduce the above copyright
  *     notice, this list of conditions and the following disclaimer in the
  *     documentation and/or other materials provided with the distribution.
  * 3. The name of the author may not be used to endorse or promote products
  *     derived from this software without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
  * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
  * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
  * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
  * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
  * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
  * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
  * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
  * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  * WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Text.RegularExpressions;
using Sharkbite.Irc;

namespace Sharkbite.Irc.Examples
{
	/// <summary>
	/// Thresher only supports the standard CTCP commands so if
	/// we were to receive a 'CTCP EXPLODE' message it would simply generate
	/// an error message. So if we want to handle these messages we simple add our
	/// own parser. We could also, for example, handle DCC VOICE messages
	/// and implement a Roger Wilco like voice commo system.   
	/// </summary>
	public class CustomParser : IParser 
	{
		private readonly Regex explodeRegex;
		private Connection connection;
	
		public CustomParser( Connection connection ) 
		{
			//This is needed to send back our reply
			this.connection = connection;

			//This regex can be used to parse any CTCP message or with a little modification
			//any message that the IRC sends back. In this example we don't care about 
			//the message body just the fact that we received a 'CTCP EXPLODE'
			explodeRegex = new Regex(":([^ ]+) [A-Z]+ [^:]+:\u0001EXPLODE([^\u0001]*)\u0001", RegexOptions.Compiled | RegexOptions.Singleline );
		}

		public bool CanParse( string line ) 
		{
			//If we return true here that means we alone get to process this message. None
			//of the other Thresher parsers will process it so no standard events will be generated.
			return explodeRegex.IsMatch( line );
		}

		public void Parse( string message )
		{
			//Let's find out who sent this message. The user is captured by
			//the ([^]+) expression.
			Match match = explodeRegex.Match( message );
			string userString = match.Groups[1].ToString();

			//Now let's convert this string into a more friendly object
			UserInfo userInfo =  Rfc2812Util.UserInfoFromString( userString );

			//Create a CTCP reply message		
			string reply = 	"NOTICE " +  userInfo.Nick + " :\x0001EXPLODE Boom!\x0001";

			//And finally send back the reply using the raw message send ability
			//of the Sender class
			connection.Sender.Raw( reply );

			//An easier way to send CTCP messages is using the CtcpSender class.
			//This would have been the friendlier:
			//connection.CtcpSender.CtcpReply("EXPLODE", userInfo.Nick,"Boom!");
			//but in this case CTCP was disabled on the Connection and raw was used
			//in its place.

		}

	}
}
