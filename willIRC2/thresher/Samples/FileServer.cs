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
using System.IO;
using System.Net;
using Sharkbite.Irc;

namespace Sharkbite.Irc.Examples
{
	/// <summary>
	///A simple DCC file server. This example shows how to send
	///a file via DCC and how to process a DCC GET request. 
	/// </summary>
	public class FileServer
	{

		private Connection connection;
		private string directory;
		private int listenPort;
		//Use a 16k buffer for transfers
		private const int BufferSize = 16384;
		//The IP address that will be sent to the client
		private string listenIP = "192.168.0.11";

		public FileServer(string fileDirectory) 
		{
			//Path to directory containing the files we wish to serve
			directory = fileDirectory;
			listenPort = 5000;
			CreateConnection();

			connection.Listener.OnError += new ErrorMessageEventHandler( OnError );
			connection.Listener.OnDisconnected += new DisconnectedEventHandler( OnDisconnected );
			connection.Listener.OnRegistered += new RegisteredEventHandler( OnRegistered );

			//We will listen for GET request then send the remote user the file
			//if it exists. 
			DccListener.DefaultInstance.OnDccGetRequest += new DccGetRequestEventHandler( OnDccFileGetRequest );
		}

		private void CreateConnection() 
		{
			string server = "sunray.sharkbite.org";
			string nick = "FileServer";
			ConnectionArgs cargs = new ConnectionArgs(nick, server);
			connection = new Connection( cargs, false, true );			
		}

		private int GetNextPort() 
		{
			//It is up to the client using Thresher to ensure that there
			//are no port confilcts.
			//Here we use a range of ports from 50k to 60k
			if( listenPort > 60000 ) 
			{
				listenPort = 50000;
			}
			return listenPort++;
		}

		public void start() 
		{
			try
			{			
				connection.Connect();
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error during connection process.");
				Console.WriteLine( e );
			}
		}
	
		public void OnRegistered() 
		{
			//We don't need to do anything here but this event signals
			//that we have connected correctly.
			Console.WriteLine( "File server connected and  registered.");
		}

		public void OnDccFileGetRequest( DccUserInfo dccUserInfo, string fileName, bool turbo ) 
		{
			try 
			{
				FileInfo file = new FileInfo( directory + "\\" + fileName );
				//See if the requested file exists
				if( !file.Exists ) 
				{
					string error = "File " + fileName  + " not found.";
					Console.WriteLine( error );
					//Send the same message to the requestor
					dccUserInfo.Connection.Sender.PrivateNotice( dccUserInfo.Nick, error );
					return;
				}

				//File was OK se we will offer the file to the remote user
				//The parameters the Send mthod takes are mostly self-explanatory. Buffer size
				//is somewhat tricky because most IRC clients expect a very small size (i.e. 1k - 4k)
				//but that slows down transfers. A size of 16k or 32k (64k works too but test
				//at your own peril) seems to produce the best results.
				//
				//Turbo indicated whether we should use the send-ahead protocol when sending the file.
				//This is much faster but is not universally supported, mIRC for example does not support it but
				//Bersirc does. 
				//
				DccFileSession session =  DccFileSession.Send( dccUserInfo, listenIP, GetNextPort(), new DccFileInfo(file), BufferSize, turbo );
		
				//Unlike the normal IRC events, DCC events can be shared by a single delegate. Of course, they
				//can be handled by separate instances so it depends on what context they are being used.
				//For the sake of simplicity we will use a single delegate to handle all the sessions' events.
				session.OnFileTransferCompleted += new FileTransferCompletedEventHandler( OnFileTransferCompleted );
				session.OnFileTransferInterrupted += new FileTransferInterruptedEventHandler( OnFileTransferInterrupted );
				session.OnFileTransferStarted += new FileTransferStartedEventHandler( OnFileTransferStarted );
				session.OnFileTransferTimeout += new FileTransferTimeoutEventHandler( OnFileTransferTimeout );
				session.OnFileTransferProgress += new FileTransferProgressEventHandler( OnFileTransferProgress );

				//Notice that we don't need to keep a reference to each session. Each session runs in its own thread
				//so we don't have to manage it manually.
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Unable to process GET request due to: " + e );
			}
		}

		public  void OnFileTransferTimeout( DccFileSession session )
		{
			//If there is no activity for the timeout period the
			//session will close. 
			Console.WriteLine( session + " timed out.");
		}

		public  void OnFileTransferStarted( DccFileSession session  )
		{
			//The connection has been made and the file has started to send
			Console.WriteLine(session + " transfer started.");
		}

		public  void OnFileTransferInterrupted( DccFileSession session  )
		{
			//The transfer was stopped for some reason before the file
			//was completely transfered.
			Console.WriteLine( session + " transfer interrupted.");
		}

		public  void OnFileTransferCompleted( DccFileSession session  )
		{
			//The file was completly sent.
			Console.WriteLine( session + " transfer completed.");
		}

		public  void OnFileTransferProgress( DccFileSession session , int bytesSent)
		{
			//This event is to support file transfer progress indicators.
			//bytesSent is the number of bytes sent in the last block. See
			//DccFileSession.ClientInfo which returns an instance of DccFileInfo for
			//other transfer status properties.
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
