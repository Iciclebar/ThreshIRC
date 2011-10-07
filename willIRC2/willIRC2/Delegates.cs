using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace willIRC2
{
    public delegate void PublicSend(string sendMessage);
    public delegate void PublicRecieve(string message);
    public delegate void NamesEventHandler(string channel, string[] nicks, bool last);
}
