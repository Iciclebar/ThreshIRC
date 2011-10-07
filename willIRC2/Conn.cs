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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Sharkbite.Irc;
using System.Threading;

namespace willIRC2
{
    class Conn
    {
      
        private Connection connection;
        private Form1 guiForm;
        
        
        ///<Summary>
        ///Connect to Designated IRC Server
        ///</Summary>

        public Conn(Form1 gui)
        {
            
            //OnRegister tells us that we have successfully established a connection with
            //the server. Once this is established we can join channels, check for people
            //online, or whatever.
            CreateConnection();
            this.guiForm = gui;
            connection.Listener.OnRegistered += new RegisteredEventHandler(OnRegistered);

            //Listen for any messages sent to the channel
            connection.Listener.OnPublic += new PublicMessageEventHandler(OnPublic);

            //Listen for bot commands sent as private messages
            connection.Listener.OnPrivate += new PrivateMessageEventHandler(OnPrivate);

            //Listen for notification that an error has ocurred 
            connection.Listener.OnError += new ErrorMessageEventHandler(OnError);

            //Listen for notification that we are no longer connected.
            connection.Listener.OnDisconnected += new DisconnectedEventHandler(OnDisconnected);

        }

        private void CreateConnection()
        {
            Identd.Stop();
            string server = "mozilla.se.eu.dal.net";
            string nick = "Will571";
            Identd.Start(nick);
            ConnectionArgs cargs = new ConnectionArgs(nick, server);
            cargs.Port = 6665;
            connection = new Connection(cargs, false, false);
        }
        
        public void start()
        {
            try
            {
                connection.Connect();
                
                AppendDelegate("IRC Connection Started");
                
            }

            catch (Exception e)
            {
               AppendDelegate("Error During Connection Process" + e);

               
                Identd.Stop();
              
            }
        }
        
        public void stop() {

            try
            {
                Identd.Stop();
                connection.Disconnect("Quitting.");
            }

            catch (Exception e)
            {
                AppendDelegate("Error During Connection Stop" + e);
               
            }
        }


        public void OnRegistered()
        {
          
            //We have to catch errors in our delegates because Thresher purposefully
            //does not handle them for us. Exceptions will cause the library to exit if they are not
            //caught.
            
            try
            {
                AppendDelegate("Connected to Server");
                //Don't need this anymore in this example but this can be left running
                //if you want.
                //Identd.Stop();

                //The connection is ready so lets join a channel.
                //We can join any number of channels simultaneously but
                //one will do for now.
                //All commands are sent to IRC using the Sender object
                //from the Connection.
              
                connection.Sender.Join("#test57");
          

            }
            catch (Exception e)
            {
                AppendDelegate("Error in OnRegistered(): " + e);
            }
        }
        public void OnPublic(UserInfo user, string channel, string message)
        {
            //Echo back any public messages
            //connection.Sender.PublicMessage(channel, user.Nick + " said, " + message);
            AppendDelegate("<" + user.Nick + "> " + message);
        }

        public void OnPrivate(UserInfo user, string message)
        {
            //Quit IRC if someone sends us a 'die' message
            if (message == "die")
            {
                connection.Disconnect("Bye");
            }
        }

        public void OnError(ReplyCode code, string message)
        {
            //All anticipated errors have a numeric code. The custom Thresher ones start at 1000 and
            //can be found in the ErrorCodes class. All the others are determined by the IRC spec
            //and can be found in RFC2812Codes.
            AppendDelegate("An error of type " + code + " due to " + message + " has occurred.");
        }

        public void OnDisconnected()
        {
            //If this disconnection was involutary then you should have received an error
            //message ( from OnError() ) before this was called.
            AppendDelegate("Connection to the server has been closed.");
        }

        void AppendDelegate(string update)
        {

            guiForm.BeginInvoke(new PublicRecieve(guiForm.AppendText), new object[] { update });

        }

 


        public void sendText(string update)
        {
          
                connection.Sender.PublicMessage("#test57", update);
        }

        public void listUsersrequest(string channel)
        {
            connection.Sender.Names(channel);
        }

        public void listUsers(string channel, string[] nicks, bool last)
        {
            
        }



       
    }
}