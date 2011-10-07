#if SSL
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
using Org.Mentalis.Security.Ssl;

namespace Sharkbite.Irc.Examples
{

	/// <summary>
	/// Identical to the Basic exmaple except this one
	/// uses an encrypted connection.
	/// </summary>
	public class Secure
	{

		/// <summary>
		/// The connection object is the focal point of the library.
		/// It used to retrieve references to the various library components.
		/// </summary>
		private Connection connection;

		/// <summary>
		/// Create a bot and register its handlers.
		/// </summary>
		public Secure() 
		{
			CreateConnection();

			//OnRegister tells us that we have successfully established a connection with
			//the server. Once this is established we can join channels, check for people
			//online, or whatever.
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
			//The hostname of the IRC server. This IRC net
			//supports SSL connections but others may not.
			string server = "ssl.axenet.org";

			//The bot's nick name on IRC
			string nick = "CryptoBot";

			//A ConnectionArgs contains all the info we need to establish
			//our connection with the IRC server and register our bot. We can still
			//use the default constructor but we need to change the
			//port to the port on the server which supports SSL (in this case 6697).
			ConnectionArgs cargs = new ConnectionArgs(nick, server);
			cargs.Port = 6697;

		
			//When creating a Connection two additional protocols may be
			//enabled: CTCP and DCC. In this example we will disable them
			//both.
			connection = new Connection( cargs, false, false );			

			//NOTE
			//We could have created multiple Connections to different IRC servers
			//and each would process messages simultaneously and independently.
			//There is no fixed limit on how many Connection can be opened at one time but
			//it is important to realize that each runs in its own Thread. Also,  separate event 
			//handlers are required for each connection, i.e. the
			//same OnRegistered() handler cannot be used for different connection
			//instances.
		}

		public void start() 
		{
			//Notice that by having the actual connect call here 
			//the constructor can add the necessary listeners before 
			//the connection process begins. If listeners are added
			//after connecting they may miss certain events. the OnRegistered()
			//event will certainly be missed.

			try
			{			
				//Connect using the TLS mode for encrypted connections.
				connection.SecureConnect();

				Console.WriteLine("CryptoBot connected via SSL.");
				//The main thread ends here but the Connection's thread is still alive.
				//We are now in a passive mode waiting for events to arrive.
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error during connection process.");
				Console.WriteLine( e );
				Identd.Stop();
			}
		}
		

		public void OnRegistered() 
		{
			//We have to catch errors in our delegates because Thresher purposefully
			//does not handle them for us. Exceptions will cause the library to exit if they are not
			//caught.
			try
			{ 
				//Don't need this anymore in this example but this can be left running
				//if you want.
				Identd.Stop();

				//The connection is ready so lets join a channel.
				//We can join any number of channels simultaneously but
				//one will do for now.
				//All commands are sent to IRC using the Sender object
				//from the Connection.
				connection.Sender.Join("#thresher");
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error in OnRegistered(): " + e ) ;
			}
		}

		public void OnPublic( UserInfo user, string channel, string message )
		{
			//Echo back any public messages
			connection.Sender.PublicMessage( channel,  user.Nick + " said, " + message );
		}

		public void OnPrivate( UserInfo user,  string message )
		{
			//Quit IRC if someone sends us a 'die' message
			if( message == "die" ) 
			{
				connection.Disconnect("Bye");
			}
		}

		public void OnError( ReplyCode code, string message) 
		{
			//All anticipated errors have a numeric code. The custom Thresher ones start at 1000 and
			//can be found in the ErrorCodes class. All the others are determined by the IRC spec
			//and can be found in RFC2812Codes.
			Console.WriteLine("An error of type " + code + " due to " + message + " has occurred.");
		}

		public void OnDisconnected() 
		{
			//If this disconnection was involutary then you should have received an error
			//message ( from OnError() ) before this was called.
			Console.WriteLine("Connection to the server has been closed.");
		}
	
	}
}
#endif