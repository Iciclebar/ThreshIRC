What Is it?
--------------
Thresher is designed to be a lightweight wrapper around the IRC protocol. 
And since IRC is completely asynchronous this means that your apps will require a somewhat 
different approach than the normal procedure/method call style. Thresher is completely event
driven; it remains in a passive mode until it receives an event.


Why?
--------
I wrote Thresher both to learn C# and to provide a relatively complete and easy to use IRC 
library for the .Net community. As far I can tell there is not currently such a library for .Net. I hope someone
will use it to write a great looking GUI client so we can give mIRC some competition. I was inspired 
by the Perl Net::IRC module and the Java RelayIRC code (all the code is, however, my own.) 


What can it do?
-------------------
Thresher understands all of the commonly used  RFC2812 commands. It does not support the odd
ones and IRC system operator commands. If you need one that is not currently included send me a 
message and I will try to add it.

Thresher also includes (at least as far as I could make sense of them ) all the CTCP commands. This includes
ACTION, PING, FINGER, etc.. However, since CTCP is really a not standard I cannot guarantee how other clients
will react to these messages. CTCP is an awful hack and I hope the IRC community will replace it with 
something more sane in the near future.

Thresher also supports the primary DCC commands, chat and send, as well as the more rare 'get' request. 


What platforms does it run on?
-------------------------------
Thresher runs on Windows 2000 and XP under Microsoft .Net 1.1. It also runs on Linux under Mono. Mono users must,
however, turn off SSL since that library is Windows specific. 



How do I use it?
------------------
There are extensive examples accompanied by notes in the 'examples' directory. 'Basic.cs' is the 
traditional quick-start example but it is recommended that you read through all of them. 
Unfortunately, only C# examples are provided.


Do you need help developing it?
-----------------------------------
Thresher is finished as far as the coding is concerned but feature requests and bug reports
are always welcome.  


Is Thresher Open Source?
-----------------------------
Yes, Thresher has been released under the GPL open source license. The Mentalis security library has a different
license (see included file.)


Software
------------
I want to thank the authors of some great software, without which I couldn't have made this library:
 
 * NUnit:  http://www.nunit.org An absolutely indispenable unit testing framework.
 * NDoc:  http://ndoc.sourceforge.net A little buggy but still very useful API doc generator. 
 * Security: http://www.mentalis.org/soft/projects/seclib/


Contributions
--------------
Special thanks to Klemen Savs for his excellent Channel mode parser.


Contact
-----------
Please send all flames, requests, and comments to:
	thresher@sharkbite.org
	
