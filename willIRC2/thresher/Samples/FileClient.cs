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
using System.Threading;
using Sharkbite.Irc;

namespace Sharkbite.Irc.Examples
{
	/// <summary>
	///A simple bot which asks for file via a DCC GET
	///request and saves to a local directory. This example
	///illustrates how to receive a file using DCC. 
	/// </summary>
	public class FileClient
	{
		private Connection connection;
		private string directory;
	
		public FileClient(String directory ) 
		{
			//Directory where downloaded files should be stored.
			this.directory = directory;
			CreateConnection();

			//Basic events
			connection.Listener.OnError += new ErrorMessageEventHandler( OnError );
			connection.Listener.OnDisconnected += new DisconnectedEventHandler( OnDisconnected );
			connection.Listener.OnRegistered += new RegisteredEventHandler( OnRegistered );

			//To receive notification that someone wishes to send us a file
			//we need to listen to this event:
			DccListener.DefaultInstance.OnDccSendRequest += new DccSendRequestEventHandler( OnDccFileTransferRequest );
		}

		private void CreateConnection() 
		{
			string server = "sunray.sharkbite.org";
			string nick = "FileClient";
			ConnectionArgs cargs = new ConnectionArgs(nick, server);
			connection = new Connection( cargs, false, true );			
		}

		public void start() 
		{
			try
			{			
				connection.Connect();

				//Loop repeatedly asking for a file on the command line
				//then downloading it.  This loop is not sync'ed to the success
				//of a download so its possible to request any number of files
				//simultaneously.
				while( true ) 
				{
					Console.Write("File Name->");
					string fileName = Console.ReadLine();
					Console.WriteLine("");
					//Send the request to "FileServer"
					DccFileSession.Get( connection, "FileServer", fileName.TrimEnd(), false );
					//If the server responds it will be at the OnDccFileTransferRequest() event.
				}
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error during connection process.");
				Console.WriteLine( e );
			}
		}

		public void OnRegistered() 
		{
			Console.WriteLine("Registered...");
		}
	
		public void OnDccFileTransferRequest(DccUserInfo dccUserInfo, string fileName, int size, bool turbo ) 
		{
			try 
			{
				//We need to pass the Receive()  method a DccFileInfo which tells Thresher
				//where to store the file. If this is an existing file that was only partially downloaded
				//then Thresher will automatically Resume the file from where it left off. Size  represents the total
				//size in bytes of the file and is provided by the sender.
				DccFileInfo dccFileInfo = new DccFileInfo( new FileInfo( directory +  "\\" + fileName ), size );
				
				//Using the DccFileInfo we created and the parameters from the event we can Receive the file.
				//A large receive buffer works the best so 32k should be good.
				DccFileSession session =  DccFileSession.Receive( dccUserInfo, dccFileInfo,  turbo );
				
				//Use the FileClient as the single source of delegates for the file sessions. 
				session.OnFileTransferCompleted += new FileTransferCompletedEventHandler( OnFileTransferCompleted );
				session.OnFileTransferInterrupted += new FileTransferInterruptedEventHandler( OnFileTransferInterrupted );
				session.OnFileTransferStarted += new FileTransferStartedEventHandler( OnFileTransferStarted );
				session.OnFileTransferTimeout += new FileTransferTimeoutEventHandler( OnFileTransferTimeout );
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error trying to receive file: " + e ) ;
			}
		}

		public  void OnFileTransferTimeout( DccFileSession session )
		{
			Console.WriteLine( session + " timed out.");
		}

		public  void OnFileTransferStarted( DccFileSession session  )
		{
			Console.WriteLine(session + " transfer started.");
		}

		public  void OnFileTransferInterrupted( DccFileSession session  )
		{
			Console.WriteLine( session + " transfer interrupted.");
		}

		public  void OnFileTransferCompleted( DccFileSession session  )
		{
			Console.WriteLine( session + " transfer completed.");
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
