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
	///Demonstrates how to establish and send messages 
	///with DCC Chat session. Show how to accept an incoming
	///chat request and how to initiate one.
	/// </summary>
	public class ChatBot
	{

	    private Connection connection;
		private string remote;
		private DccChatSession chatSession;

		public ChatBot() 
		{
			CreateConnection();
		
			//Standard delegates
			connection.Listener.OnRegistered += new RegisteredEventHandler( OnRegistered );
			connection.Listener.OnError += new ErrorMessageEventHandler( OnError );
			connection.Listener.OnDisconnected += new DisconnectedEventHandler( OnDisconnected );
			//This one is need to test if someone is online
			connection.Listener.OnIson += new IsonEventHandler( OnIson );

			//Listening for an incoming chat request is different than the other kinds of messgages
			//we normally receive. DCC actions are independant of any particular connection but
			//at the same time normal IRC messages are used to initiate them. So in order to
			//be notified of incoming request we get a reference to the singleton instance
			//of DccListener and add our delegate to it.
			DccListener.DefaultInstance.OnDccChatRequest += new DccChatRequestEventHandler( OnDccChatRequest );

		}

		private void CreateConnection() 
		{
				//The hostname of the IRC server
				string server = "sunray.sharkbite.org";

				//The bot's nick on IRC
				string nick = "ChatBot";
				
				//The nick to chat with
				remote = "Admin";

				ConnectionArgs cargs = new ConnectionArgs(nick, server);
				//Enable DCC
				connection = new Connection( cargs, false, true );			
		}

		private void RegisterDelegates() 
		{
			//Now that we have a session instance we can add delegates to it. There are only
			//4 kinds of events raised by a DccChatSession. A timeout is only raised when we are the initiator
			//and someone fails to respond to our request within the given time.
			chatSession.OnChatSessionOpened += new ChatSessionOpenedEventHandler( OnChatSessionOpened );
			chatSession.OnChatSessionClosed += new ChatSessionClosedEventHandler( OnChatSessionClosed );
			chatSession.OnChatMessageReceived += new ChatMessageReceivedEventHandler( OnChatMessageReceived );
			chatSession.OnChatRequestTimeout += new ChatRequestTimeoutEventHandler( OnChatRequestTimeout );
		}

		private void InitiateChat( string nick ) 
		{
			//Initiating a chat is only slightly more complicated than accepting one.
			
			//First we must create a DccUserInfo instance which contains the information
			//on how to send the request, i.e. what connection to use and who should receive it.
			DccUserInfo userInfo = new DccUserInfo( connection, nick );	

			//Now we need to send our IP address and the port we will be listening on. Those
			//behind a NAT firewall should read the API docs for more information on what to use
			//as an IP address. The port can be any value above 1024. Thresher does not manage these
			//so it is up to us not to pick a port already in use. 
			//Request() can take other arguments including a crypto protocol. See the API
			//docs for more information.
			chatSession =  DccChatSession.Request( userInfo, "192.168.0.11", 50000, 30000 );
			
			//Add delegates
			RegisterDelegates();

			//At this point one of two things will happen: the remote user will accept
			//and OnChatSessionOpened() will be called or the user will not respond
			//and OnChatRequestTimeout() will be called instead.

			//The new session Thread is now running and waiting for a response...

		}

		public void start() 
		{
			try
			{			
				connection.Connect();
				Console.WriteLine("ChatBot connected.");
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error during connection process.");
				Console.WriteLine( e );
			}
		}
		

		public void OnRegistered() 
		{
			//Check if someone is online
			connection.Sender.Ison( remote );
		}
		
		public void OnIson( string nick ) 
		{
			//If the remote use was actually online then try to establish a chat
			//session. Otherwise just wait for someone else to initiate it.
			if( nick != "" ) 
			{
				InitiateChat( nick );
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


		public void OnDccChatRequest( DccUserInfo dccUserInfo ) 
		{
			//This event will be raised if someone is attempting to initiate a Chat session.
			//DccUserInfo extends UserInfo and contains some additional networking information
			//as well as the Connection instance used to send the request. 

			//There are lots of things which can go wrong with DCC so the creation of any
			//type of DCC session should always be in a try/catch block.
			try 
			{
				Console.WriteLine("Chat request from " + dccUserInfo.Nick ) ;

				//Untill we call accept on the session it will not be opened so we could ignore
				//it if we wanted to. The Accept parameters  are the same ones kindly provided
				//by the ChatRequest event.
				chatSession =  DccChatSession.Accept( dccUserInfo );
				
				//Now add delegate to the session
				RegisterDelegates();

				//All DCC sessions are direct connections to other users and do not pass through
				//the IRC server. The IRC server is only a means of sending session requests. DCC 
				//sessions also run in their own separate Threads, one for each session instance.
				//When a session is closed the Thread ends.

				//Since we don't want to be notified of any more chat request we will
				//remove ourselves form the list. It is always important to remove yourself from
				//events when they are no longer needed.
				DccListener.DefaultInstance.OnDccChatRequest -= new DccChatRequestEventHandler( OnDccChatRequest );

				//Now we wait for messages...
			
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Exception handling Chat request: " + e);
			}
		}
	
		public void OnChatMessageReceived( DccChatSession session, string message ) 
		{
			Console.WriteLine("Chat Message Received: " + message);
			if( message == "die" ) 
			{
				//Close() closes the socket and stops the session Thread
				session.Close();
			}
			else 
			{
				//Use sendMessage() to send text to the remote user. Newlines
				//will automatically be appended to the message before sending.
				//Here, like the other examples, we simply echo back any text sent to us.
				session.SendMessage(  message );
			}
		}

		public void OnChatSessionOpened( DccChatSession session ) 
		{
			Console.WriteLine("Chat session opened.");
		}

		public void OnChatSessionClosed( DccChatSession session ) 
		{
			Console.WriteLine("Chat session closed.");

			//When the chat session closes then close the IRC connection to.
			connection.Disconnect("No time for chat.");
		}

		public void OnChatRequestTimeout( DccChatSession session ) 
		{
			//When a session times out it is closed and cannot be reused.
			//We must create a new one.
			Console.WriteLine("Chat request timed out.");
		}




	}
}
