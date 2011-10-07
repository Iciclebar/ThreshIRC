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
	public class Launcher
	{

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				if( args.Length < 1 ) 
				{
					Console.WriteLine("Please select one of the following numbers:");
					Console.WriteLine("\t1. Basic connection example");
					Console.WriteLine("\t2. Intermediate example using CTCP");
					Console.WriteLine("\t3. Advanced example with a custom parser");
					Console.WriteLine("\t4. DCC Chat example");
					Console.WriteLine("\t5. Simple DCC file server (needs directory as second arg)");
					Console.WriteLine("\t6. DCC file downloader (needs directory as second arg)");
					#if SSL
					Console.WriteLine("\t7. Secure connection example");
					#endif
					return;
				}
				int choice = int.Parse( args[0] ); 
				
				switch( choice ) 
				{
					case 1:
						Basic basic = new Basic();
						basic.start();
						break;
					case 2:
						Intermediate intermediate = new Intermediate();
						intermediate.start();
						break;
					case 3:
						Advanced  advanced = new Advanced();
						advanced.start();
						break;
					case 4:
						ChatBot chatBot = new ChatBot();
						chatBot.start();
						break;
					case 5:
						FileServer fileServer = new FileServer( args[1] );
						fileServer.start();
						break;
					case 6:
						FileClient fileClient = new FileClient( args[1] );
						fileClient.start();
						break;
					#if SSL
					case 7:
						Secure secure = new Secure();
						secure.start();
						break;
					#endif
					default:
					#if SSL
							Console.WriteLine("Please choose an example form 1 to 7.");
					#else
							Console.WriteLine("Please choose an example form 1 to 6.");
					#endif
					break;
				}
			}
			catch( FormatException fe ) 
			{
				Console.WriteLine("The first argument must be a number.");
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Unanticipated exception " + e );
			}
		}

	}
}
