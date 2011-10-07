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
using Sharkbite.Irc;

namespace Sharkbite.Irc.Examples
{

	/// <summary>
	/// This one enables CTCP queries. 
	/// </summary>
	public class Intermediate
	{

		/// <summary>
		/// The connection object is the focal point of the library.
		/// It used to retrieve references to the various library components.
		/// </summary>
		private Connection connection;

		/// <summary>
		/// Create a bot and register its handlers.
		/// </summary>
		public Intermediate() 
		{
			CreateConnection();

			//OnRegister tells us that we have successfully established a connection with
			//the server. Once this is established we can join channels, check for people
			//online, or just sit back and take up some bandwidth.
			connection.Listener.OnRegistered += new RegisteredEventHandler( OnRegistered );
						
			//The auto-responder will handle this for us but let's see what people send us anyway.
			//This will pick up all CTCP requests except for CTCP Ping which has its own event.
			connection.CtcpListener.OnCtcpRequest+= new CtcpRequestEventHandler( OnOnCtcpRequest );

			//Listen for any messages sent to the channel
			connection.Listener.OnPublic += new PublicMessageEventHandler( OnPublic );

			//Listen for bot commands sent as private messages
			connection.Listener.OnPrivate += new PrivateMessageEventHandler( OnPrivate );
	
			//Listen for notification that an error has ocurred 
			connection.Listener.OnError += new ErrorMessageEventHandler( OnError );

			//Listen for notification that we are no longer connected.
			connection.Listener.OnDisconnected += new DisconnectedEventHandler( OnDisconnected );
		}

		private void CreateConnection() 
		{
			//The hostname of the IRC server
			string server = "sunray.sharkbite.org";

			//The bot's nick on IRC
			string nick = "EchoBot";

			//A ConnectionArgs contains all the info we need to establish
			//our connection with the IRC server and register our bot. 
			ConnectionArgs cargs = new ConnectionArgs(nick, server);

			//We want to enable CTCP but not DCC for this bot.
			connection = new Connection( cargs, true, false );

			//CtcpResponder will automatically reply to CTCP queries using
			//either default values or ones you set. You can also set
			//the minimum interval between responses so that the bot
			//cannot be tricked into flooding IRC.
			CtcpResponder autoResponder = new CtcpResponder( connection );
			autoResponder.UserInfoResponse = "A simple but friendly bot.";
			autoResponder.ClientInfoResponse = "EchoBot";
			//The minimum interval is in milliseconds.
			autoResponder.ResponseDelay = 1000;

			//To enable this responder the connection must have
			//its CTCPResponder property set to an instance.
			//It can be turned off later by setting the CTCPResponder
			//property to null;
			connection.CtcpResponder = autoResponder;
		}

		public void start() 
		{
			try
			{
				//Open the connection	
				connection.Connect();

				Console.WriteLine("Echobot connected.");
				//The main thread ends here but the Connection's thread is still alive.
				//We are now in a passive mode waiting for events to arrive.
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error during connection process.");
				Console.WriteLine( e );
			}
		}


		public void OnRegistered() 
		{
			//Join our favorite channel
			connection.Sender.Join("#thresher");
		}

		public void OnPublic( UserInfo user, string channel, string message )
		{
			connection.Sender.PublicMessage( channel,  user.Nick + " said, " + message );
		}

		public void OnPrivate( UserInfo user,  string message )
		{
			if( message == "die" ) 
			{
				connection.Disconnect("Bye");
			}
		}

		public void OnError( ReplyCode code, string message) 
		{
			Console.WriteLine("An error of type " + code + " due to " + message + " has occurred.");
		}

		public void OnDisconnected() 
		{
			Console.WriteLine("Connection to the server has been closed.");
		}

		public void OnOnCtcpRequest(string command, UserInfo who ) 
		{
			//Take a look at any CTCP request we get.
			Console.WriteLine(who.Nick +  " sent a CTCP " + command + " request.");

			//If we wanted  we could reply to it manually by calling:
			//connection.CtcpSender.CtcpReply( command, who.Nick, "a reply");
			//This also allows us to send an aribitrary CTCP command.
		}
	
	}
}
