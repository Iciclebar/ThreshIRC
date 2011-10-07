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
	/// This example demonstrates the use of a custom parser
	/// for handling an unknown message type.
	/// </summary>
	public class Advanced
	{

		private Connection connection;

		/// <summary>
		/// Create a bot and register its handlers.
		/// </summary>
		public Advanced() 
		{
			CreateConnection();

			//OnRegister tells us that we have successfully established a connection with
			//the server. Once this is established we can join channels, check for people
			//online, or just sit back and take up some bandwidth.
			connection.Listener.OnRegistered += new RegisteredEventHandler( OnRegistered );
						
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
			string server = "washington.dc.us.undernet.org";
			string nick = "UltraBot";
			ConnectionArgs cargs = new ConnectionArgs(nick, server);
			connection = new Connection( cargs, false, false );
			//We don't need to enable CTCP because we are using our own
			//custom parser.
		
			//Let's add a custom parser to this connection.
			CustomParser parser = new CustomParser( connection );
			connection.AddParser( parser );

			//Custom parsers are intended to allow developers to handle messages
			//not already handled by Thresher itself.  For example, XDCC support 
			//or some odd DCC type like Voice could be added.
			//There is an event, Connection.OnRawMessageReceived(), which
			//allows developers to receive  unparsed messages from the IRC server.
			//But though this can be used in a manner similar to a custom parser
			//the new parser mechanism is cleaner and more efficient.
		}

		public void start() 
		{
			try
			{
				connection.Connect();
				Console.WriteLine("UltraBot connected.");
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
	
	}
}
