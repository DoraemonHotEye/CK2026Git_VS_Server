using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public IPEndPoint PeerEndPoint { get; set; }
    public ClientSession session { get; set; }
    public Room CurrentRoom { get; set; }
}

